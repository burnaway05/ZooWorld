using Gameplay.Definitions;
using System;
using UnityEngine;

namespace Gameplay.Animals
{
    public class Animal
    {
        private readonly IAnimalMovement _movement;

        public AnimalDefinition Definition { get; private set; }
        public IAnimalBody Body { get; private set; }
        public bool IsAlive { get; private set; }
        public AnimalType Type => Definition.Type;
        public Vector3 Forward => Body.Forward;
        public Vector3 Position => Body.Position;
        public event Action Ate;

        public Animal(AnimalDefinition definition, IAnimalBody view)
        {
            Definition = definition;
            Body = view;
            IsAlive = true;

            _movement = Definition.CreateMovement(Body);
        }

        public void FixedTick(float deltaTime)
        {
            if (!IsAlive)
            {
                return;
            }

            _movement.FixedTick(deltaTime);
        }

        public void TurnAround()
        {
            Vector3 dirToOrigin = -Body.Position.normalized;
            var rotation = Quaternion.LookRotation(dirToOrigin).eulerAngles;

            Body.MoveRotation(Quaternion.Euler(new Vector3(rotation.x, rotation.y + UnityEngine.Random.Range(-30, 30), rotation.z)));
        }

        public void Eat()
        {
            Ate?.Invoke();
        }

        public void Kill()
        {
            IsAlive = false;
        }
    }
}