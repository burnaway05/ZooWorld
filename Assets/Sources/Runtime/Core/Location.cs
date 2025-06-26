using System.Collections.Generic;
using Assets.Sources.Runtime.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Runtime.Core
{
    internal class Location
    {
        private GameObject _locationView;
        private GameConfig _gameConfig;
        private EnemyFactory _enemyFactory;

        private List<Animal> _animals;
        private Rect _spawnArea;

        public Location(GameObject locationView, GameConfig gameConfig, AssetService assetService)
        {
            _locationView = locationView;
            _gameConfig = gameConfig;
            _enemyFactory = new EnemyFactory(assetService);
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
                _animals.Add(_enemyFactory.Create(_gameConfig.AnimalsConfig[GetRandomIndexAnimal()].Name, GetRandomPosition(), GetRandomRotation()));
                await UniTask.Delay(System.TimeSpan.FromSeconds(_gameConfig.SpawnAnimalInterval));
            }
        }

        private int GetRandomIndexAnimal()
        {
            return Random.Range(0, _gameConfig.AnimalsConfig.Length);
        }

        private Quaternion GetRandomRotation()
        {
            return Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
        }

        private Vector3 GetRandomPosition()
        {
            float x = Random.Range(-_spawnArea.width / 2, _spawnArea.width / 2);
            float z = Random.Range(-_spawnArea.height / 2, _spawnArea.height / 2);
            float y = 0.5f;

            return new Vector3(x, y, z);
        }
    }
}