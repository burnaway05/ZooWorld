using System.Collections.Generic;
using Assets.Sources.Runtime.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Location
{
    private GameObject _locationView;
    private GameConfig _gameConfig;

    private List<Animal> _animals;
    private List<GameObject> _animalViews;
    private Rect _spawnArea;

    public Location(GameObject locationView, GameConfig gameConfig)
    {
        _locationView = locationView;
        _gameConfig = gameConfig;
        _animals = new List<Animal>();
        _animalViews = new List<GameObject>();
        CreateSpawnArea();
        SpawnAnimalsLoop();
    }

    private void CreateSpawnArea()
    {
        _spawnArea = new Rect(_locationView.transform.localPosition, _locationView.transform.localScale);
    }

    private async void SpawnAnimalsLoop()
    {
        foreach (var animalConfig in _gameConfig.AnimalsConfig)
        {
            var animalPrefab = await LoadFromReference(animalConfig.View);
            _animalViews.Add(animalPrefab);
        }

        while (true)
        {
            float x = Random.Range(-_spawnArea.width / 2, _spawnArea.width / 2);
            float z = Random.Range(-_spawnArea.height / 2, _spawnArea.height / 2);
            float y = 0.5f;
            var position = new Vector3(x, y, z);

            Object.Instantiate(_animalViews[Random.Range(0, _animalViews.Count - 1)], position, Quaternion.identity);
            await UniTask.Delay(System.TimeSpan.FromSeconds(_gameConfig.SpawnAnimalInterval));
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
}