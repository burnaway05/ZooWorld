using Cysharp.Threading.Tasks;
using Gameplay.Definitions;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Gameplay.Animals
{
    public class AnimalFactory
    {
        private readonly Dictionary<Animal, AnimalView> _instances;
        private readonly ObjectPool _pool;

        public AnimalFactory(ObjectPool pool)
        {
            _instances = new Dictionary<Animal, AnimalView>();
            _pool = pool;
        }

        public async UniTask<Animal> CreateAsync(AnimalDefinition definition, Vector3 position, Quaternion rotation, IAnimalCollisionHandler collisionHandler, CancellationToken cancellationToken)
        {
            var gameObject = await _pool.GetAsync(definition.Prefab, cancellationToken);
            gameObject.transform.localPosition = position;
            gameObject.transform.localRotation = rotation;

            var view = gameObject.GetComponent<AnimalView>();
            view.Bind(collisionHandler);
            var animal = new Animal(definition, view);

            _instances.Add(animal, view);
            return animal;
        }

        public void Release(Animal animal)
        {
            var view = _instances[animal];
            view.Unbind();
            _pool.Return(animal.Definition.Prefab, view.gameObject);
            _instances.Remove(animal);
        }
    }
}