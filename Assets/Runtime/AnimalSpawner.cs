using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AnimalSpawner
{
    private GameDefinition _gameDefinition;

    public AnimalSpawner(GameDefinition gameDefinition)
    {
        _gameDefinition = gameDefinition;
    }

    public async UniTask SpawnAsync(CancellationToken cancellationToken)
    {
        while(!cancellationToken.IsCancellationRequested)
        {
            var delay = Random.Range(_gameDefinition.MinSpawnnterval, _gameDefinition.MaxSpawnnterval);

            await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);

            var animal = GetAnimal();
            Debug.LogWarning($"Spawn {animal.Name}");
        }
    }

    private AnimalDefinition GetAnimal()
    {
        var animals = _gameDefinition.Animals;
        var randomAnimal = animals[Random.Range(0, animals.Length)];

        return randomAnimal;
    }

    private Vector3 GetSpawnPosition()
    {
        return Vector3.zero;
    }

    private Quaternion GetSpawnRotatin()
    {
        return Quaternion.identity;
    }
}
