using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Location
{
    private GameConfig _gameConfig;

    public Location(GameConfig gameConfig)
    {
        _gameConfig = gameConfig;
    }

    public void SpawnAnimal()
    {
        Addressables.LoadAssetAsync<GameObject>(_gameConfig.AnimalsConfig[1].View).Completed += OnLoaded;
    }

    private void OnLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Vector3 position = new Vector3(0, 0);
            Object.Instantiate(handle.Result, position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Asset is not loaded");
        }
    }
}