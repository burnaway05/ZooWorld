using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Runtime.Core
{
    internal class Location
    {
        private GameObject _locationView;
        private GameConfig _gameConfig;
        private AssetService _assetService;

        private List<Animal> _animals;
        private Rect _spawnArea;

        public Location(GameObject locationView, GameConfig gameConfig, AssetService assetService)
        {
            _locationView = locationView;
            _gameConfig = gameConfig;
            _assetService = assetService;
            _animals = new List<Animal>();
            InitializeSpawnArea();
            PeriodicSpawnAnimals().Forget();
        }

        private void InitializeSpawnArea()
        {
            _spawnArea = new Rect(_locationView.transform.localPosition, _locationView.transform.localScale);
        }

        private async UniTaskVoid PeriodicSpawnAnimals()
        {
            while(true)
            {
                var animalView = _assetService.Get(_gameConfig.AnimalsConfig[Random.Range(0, _gameConfig.AnimalsConfig.Length)].Name);
                animalView.transform.localPosition = GetRandomPoint();
                _animals.Add(new Frog(animalView));
                await UniTask.Delay(System.TimeSpan.FromSeconds(_gameConfig.SpawnAnimalInterval));
            }
        }

        private Vector3 GetRandomPoint()
        {
            float x = Random.Range(-_spawnArea.width / 2, _spawnArea.width / 2);
            float z = Random.Range(-_spawnArea.height / 2, _spawnArea.height / 2);
            float y = 0.5f;

            return new Vector3(x, y, z);
        }
    }
}