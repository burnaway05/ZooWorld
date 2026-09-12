using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AnimalFactory
{
    private readonly Dictionary<Animal, GameObject> _instances;
    private readonly ObjectPool _pool;

    public AnimalFactory(ObjectPool pool)
    {
        _instances = new Dictionary<Animal, GameObject>();
        _pool = pool;
    }

    public async UniTask<Animal> CreateAsync(AnimalDefinition definition, Vector3 position, Quaternion rotation, CancellationToken cancellationToken)
    {
        var gameObject = await _pool.GetAsync(definition.Prefab, cancellationToken);
        gameObject.transform.localPosition = position;
        gameObject.transform.localRotation = rotation;

        var view = gameObject.GetComponent<AnimalView>();

        var animal = new Animal(definition, view);

        _instances.Add(animal, gameObject);
        return animal;
    }

    public void Release(Animal animal)
    {
        _pool.Return(animal.Definition.Prefab, _instances[animal]);
        _instances.Remove(animal);
    }
}
