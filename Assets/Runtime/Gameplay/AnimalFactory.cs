using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AnimalFactory
{
    private ObjectPool _pool;

    public AnimalFactory(ObjectPool pool)
    {
        _pool = pool;
    }

    public async UniTask<Animal> CreateAsync(AnimalDefinition definition, Vector3 position, Quaternion rotation, CancellationToken cancellationToken)
    {
        var gameObject = await _pool.GetAsync(definition.Prefab, cancellationToken);
        gameObject.transform.localPosition = position;
        gameObject.transform.localRotation = rotation;

        var view = gameObject.GetComponent<AnimalView>();

        var animal = new Animal(definition, view);
        return animal;
    }

    public void Release(Animal animal)
    {
        _pool.Return(animal.Definition.Prefab, animal.View.gameObject);
    }
}
