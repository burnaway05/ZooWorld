using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AnimalSpawner
{
    private GameDefinition _gameDefinition;
    private AnimalFactory _factory;

    public AnimalSpawner(GameDefinition gameDefinition, AnimalFactory factory)
    {
        _gameDefinition = gameDefinition;
        _factory = factory;
    }

    public async UniTask<Animal> SpawnAsync(Vector3 position, Quaternion rotation, CancellationToken cancellationToken)
    {
        var delay = Random.Range(_gameDefinition.MinSpawnnterval, _gameDefinition.MaxSpawnnterval);

        await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);

        var definition = GetDefiniton();
        var animal = await _factory.CreateAsync(definition, position, rotation, cancellationToken);

        return animal;
    }

    private AnimalDefinition GetDefiniton()
    {
        var animals = _gameDefinition.Animals;
        var randomAnimal = animals[Random.Range(0, animals.Length)];

        return randomAnimal;
    }
}
