using UnityEngine;

namespace Gameplay.Animals
{
    public class LinearMovement : IAnimalMovement
    {
        public readonly float _speed;

        private IAnimalBody _body;

        public LinearMovement(float speed, IAnimalBody body)
        {
            _speed = speed;
            _body = body;
        }

        public void FixedTick(float deltaTime)
        {
            _body.Rigidbody.linearVelocity = _body.Forward * _speed + Vector3.up * _body.Rigidbody.linearVelocity.y;
        }
    }
}