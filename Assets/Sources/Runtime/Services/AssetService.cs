using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

namespace Assets.Sources.Runtime.Core
{
    internal class AssetService
    {
        private const int size = 10;
        private GameConfig _gameConfig;
        private Dictionary<string, GameObject> _prefabs;
        private Dictionary<string, Queue<GameObject>> _instances;

        public AssetService(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _prefabs = new Dictionary<string, GameObject>();
            _instances = new Dictionary<string, Queue<GameObject>>();
        }

        public async UniTask WarmUpPrefabs()
        {
            foreach (var animalConfig in _gameConfig.AnimalsConfig)
            {
                var animalPrefab = await LoadFromReference(animalConfig.View);
                _prefabs.Add(animalConfig.Name, animalPrefab);
                _instances.Add(animalConfig.Name, new Queue<GameObject>(size));

                for (int i = 0; i < size; i++)
                {
                    var obj = Object.Instantiate(animalPrefab, Vector3.zero, Quaternion.identity);
                    obj.SetActive(false);
                    _instances[animalConfig.Name].Enqueue(obj);
                }
            }
        }

        private async UniTask<GameObject> LoadFromReference(AssetReference reference)
        {
            var handle = reference.LoadAssetAsync<GameObject>();
            await handle.ToUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            Debug.LogError("Resource has not been loaded");
            return null;
        }

        public GameObject Get(string name)
        {
            if(!_instances.ContainsKey(name))
            {
                Debug.LogWarning("Prefab not found");
                return null;
            }

            GameObject obj;

            if (_instances[name].Count > 0)
            {
                obj = _instances[name].Dequeue();
            }
            else
            {
                obj = Object.Instantiate(_prefabs[name]);
            }

            obj.SetActive(true);
            return obj;
        }

        public void Return(string name, GameObject obj)
        {
            obj.SetActive(false);
            _instances[name].Enqueue(obj);
        }
    }
}
