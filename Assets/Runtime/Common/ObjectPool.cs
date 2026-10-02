using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ZooWorld.Pooling
{
    public class ObjectPool : IDisposable
    {
        public static string Status;
        private Transform _root;
        private readonly HashSet<GameObject> _instances;
        private Dictionary<AssetReference, Queue<GameObject>> _pool;

        public ObjectPool()
        {
            _instances = new HashSet<GameObject>();
            _pool = new Dictionary<AssetReference, Queue<GameObject>>();
        }

        private Transform GetRoot()
        {
            if (_root != null)
            {
                return _root;
            }

            var rootObject = new GameObject("ObjectPool");
            _root = rootObject.transform;

            return _root;
        }

        public async UniTask<GameObject> GetAsync(AssetReference reference, CancellationToken cancellationToken)
        {
            var pool = GetOrAddPool(reference);
            GameObject instance = null;
            if (pool.Count > 0)
            {
                instance = _pool[reference].Dequeue();
            }
            else
            {
                instance = await CreateAsync(reference, cancellationToken);
            }
            instance.gameObject.SetActive(true);

            return instance;
        }

        public void Return(AssetReference reference, GameObject instance)
        {
            instance.gameObject.SetActive(false);
            _pool[reference].Enqueue(instance);
        }

        private async UniTask<GameObject> CreateAsync(AssetReference reference, CancellationToken cancellationToken)
        {
            var sizeHandle = Addressables.GetDownloadSizeAsync(reference.RuntimeKey);
            await sizeHandle.Task;
            if (sizeHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Status += "'n" + $"[Addressables] Download size: {sizeHandle.Result / 1024f / 1024f:F2} MB";
                Debug.Log($"[Addressables] Download size: {sizeHandle.Result / 1024f / 1024f:F2} MB");
            }
            else
            {
                Status += "'n" + $"[Addressables] Couldn't get download size: {sizeHandle.OperationException}";
                Debug.LogError($"[Addressables] Couldn't get download size: {sizeHandle.OperationException}");
            }

            Addressables.Release(sizeHandle);

            var handle = Addressables.InstantiateAsync(reference, GetRoot(), false, true);

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Status += "'n" + $"[Addressables] SUCCESS: {reference.RuntimeKey}";
                Debug.Log($"[Addressables] SUCCESS: {reference.RuntimeKey}");
            }
            else
            {
                Status += "'n" + $"[Addressables] FAILED: {reference.RuntimeKey}\n" + $"{handle.OperationException}";
                Debug.LogError($"[Addressables] FAILED: {reference.RuntimeKey}\n" + $"{handle.OperationException}");
            }

            var gameObject = 
            _instances.Add(handle.Result);

            if (cancellationToken.IsCancellationRequested)
            {
                Addressables.ReleaseInstance(handle.Result);
                cancellationToken.ThrowIfCancellationRequested();
            }

            return handle.Result;
        }

        public async UniTask WarmUpAsync(AssetReference reference, int count, CancellationToken cancellationToken)
        {
            var pool = GetOrAddPool(reference);

            while (pool.Count < count)
            {
                var instance = await CreateAsync(reference, cancellationToken);

                instance.SetActive(false);
                pool.Enqueue(instance);
            }
        }

        private Queue<GameObject> GetOrAddPool(AssetReference reference)
        {
            if (!_pool.ContainsKey(reference))
            {
                _pool.Add(reference, new Queue<GameObject>());
            }

            return _pool[reference];
        }

        public void Dispose()
        {
            foreach (var instance in _instances)
            {
                if (instance != null)
                {
                    Addressables.ReleaseInstance(instance);
                }
            }

            _instances.Clear();

            if (_root != null)
            {
                UnityEngine.Object.Destroy(_root.gameObject);
                _root = null;
            }
        }
    }
}