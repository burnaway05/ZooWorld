using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ZooWorld.Pooling
{
    public class ObjectPool : IDisposable
    {
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
            var gameObject = await Addressables.InstantiateAsync(reference, GetRoot(), false, true);
            _instances.Add(gameObject);

            if (cancellationToken.IsCancellationRequested)
            {
                Addressables.ReleaseInstance(gameObject);
                cancellationToken.ThrowIfCancellationRequested();
            }

            return gameObject;
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