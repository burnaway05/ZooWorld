using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Gameplay.Definitions;
using Gameplay.Animals;
using VContainer.Unity;

namespace Gameplay.Game
{
    public class Location : IAnimalCollisionHandler
    {
        private List<Animal> _animals;
        private LocationView _view;

        private GameDefinition _gameDefinition;
        private AnimalFactory _factory;
        private GameStatistics _gameStatistics;

        public Location(LocationView view, GameDefinition gameDefinition, AnimalFactory factory, GameStatistics gameStatistics)
        {
            _animals = new List<Animal>();
            _view = view;
            _gameDefinition = gameDefinition;
            _factory = factory;
            _gameStatistics = gameStatistics;
        }

        public void AddAnimal(Animal animal)
        {
            _animals.Add(animal);
        }

        private AnimalDefinition GetDefiniton()
        {
            var animals = _gameDefinition.Animals;
            var randomAnimal = animals[Random.Range(0, animals.Length)];

            return randomAnimal;
        }

        public Vector3 GetSpawnPosition()
        {
            var position = new Vector3(
                   Random.Range(_view.Bounds.min.x, _view.Bounds.max.x),
                   _view.Bounds.max.y,
                   Random.Range(_view.Bounds.min.z, _view.Bounds.max.z));

            return position;
        }

        public Quaternion GetSpawnRotatin()
        {
            return Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
        }

        public void Tick()
        {
            for (var i = _animals.Count - 1; i >= 0; i--)
            {
                if (!_animals[i].IsAlive)
                {
                    _factory.Release(_animals[i]);
                    _animals.RemoveAt(i);
                }
            }
        }

        public void FixedTick()
        {
            var deltaTime = Time.fixedDeltaTime;
            foreach (var animal in _animals)
            {
                if (IsNeedToTurnAround(animal))
                {
                    animal.TurnAround();
                }

                animal.FixedTick(deltaTime);
            }
        }

        public async UniTask<Animal> SpawnAsync(Vector3 position, Quaternion rotation, CancellationToken cancellationToken)
        {
            var delay = Random.Range(_gameDefinition.MinSpawnnterval, _gameDefinition.MaxSpawnnterval);

            await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);

            var definition = GetDefiniton();
            var animal = await _factory.CreateAsync(definition, position, rotation, this, cancellationToken);

            return animal;
        }

        public bool IsNeedToTurnAround(Animal animal)
        {
            return IsAnimalOutOfScreen(animal) && !IsAnimalLookInside(animal);
        }

        private bool IsAnimalOutOfScreen(Animal animal)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(animal.Position);

            bool visible =
                screenPos.z > 0 &&
                screenPos.x >= 0 && screenPos.x <= Screen.width &&
                screenPos.y >= 0 && screenPos.y <= Screen.height;

            return !visible;
        }

        private bool IsAnimalLookInside(Animal animal)
        {
            Vector3 dirToOrigin = -animal.Position.normalized;
            float dot = Vector3.Dot(animal.Forward, dirToOrigin);

            return dot > 0;
        }

        public void HandleCollision(IAnimalBody first, IAnimalBody second)
        {
            Animal firstAnimal = null;
            Animal secondAnimal = null;
            foreach (var animal in _animals)
            {
                if (animal.Body == first)
                {
                    firstAnimal = animal;
                }

                if (animal.Body == second)
                {
                    secondAnimal = animal;
                }
            }

            ResolveInteraction(firstAnimal, secondAnimal);
        }

        private void ResolveInteraction(Animal first, Animal second)
        {
            if (first == null || second == null)
            {
                return;
            }

            if (!first.IsAlive || !second.IsAlive)
            {
                return;
            }

            if (first.Definition.Type == AnimalType.Prey && second.Definition.Type == AnimalType.Prey)
            {
                var pushImpulse = 2;
                var direction = (first.Body.Position - second.Body.Position).normalized;

                first.Body.AddForce(direction * pushImpulse, ForceMode.VelocityChange);

                second.Body.AddForce(-direction * pushImpulse, ForceMode.VelocityChange);
            }

            if (first.Definition.Type == AnimalType.Prey && second.Definition.Type == AnimalType.Predator)
            {
                _gameStatistics.RegisterPreyDeath();
                Kill(first);
                second.Eat();
            }

            if (first.Definition.Type == AnimalType.Predator && second.Definition.Type == AnimalType.Prey)
            {
                _gameStatistics.RegisterPreyDeath();
                Kill(second);
                first.Eat();
            }

            if (first.Definition.Type == AnimalType.Predator && second.Definition.Type == AnimalType.Predator)
            {
                _gameStatistics.RegisterPredatorDeath();
                Kill(first);
                second.Eat();
            }
        }

        private void Kill(Animal animal)
        {
            _factory.Release(animal);
            _animals.Remove(animal);
        }
    }
}