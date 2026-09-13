using Gameplay.Definitions;
using UnityEngine;

namespace Gameplay.Animals
{
    public class Animal
    {
        private IAnimalMovement _movement;

        public AnimalDefinition Definition { get; private set; }
        public IAnimalBody View { get; private set; }
        public bool IsAlive { get; private set; }
        public AnimalType Type => Definition.Type;

        public Animal(AnimalDefinition definition, IAnimalBody view)
        {
            Definition = definition;
            View = view;
            IsAlive = true;

            _movement = Definition.CreateMovement(View);
        }

        public void FixedTick(float deltaTime)
        {
            _movement.FixedTick(deltaTime);
        }

        public bool IsNeedToTurnAround()
        {
            return IsAnimalOutOfScreen() && !IsAnimalLookInside();
        }

        private bool IsAnimalOutOfScreen()
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(View.Position);

            bool visible =
                screenPos.z > 0 &&
                screenPos.x >= 0 && screenPos.x <= Screen.width &&
                screenPos.y >= 0 && screenPos.y <= Screen.height;

            return !visible;
        }

        private bool IsAnimalLookInside()
        {
            Vector3 dirToOrigin = -View.Position.normalized;
            float dot = Vector3.Dot(View.Forward, dirToOrigin);

            return dot > 0;
        }

        public virtual void TurnAround()
        {
            Vector3 dirToOrigin = -View.Position.normalized;
            var rotation = Quaternion.LookRotation(dirToOrigin).eulerAngles;

            View.Rigidbody.MoveRotation(Quaternion.Euler(new Vector3(rotation.x, rotation.y + Random.Range(-30, 30), rotation.z)));
        }
    }
}