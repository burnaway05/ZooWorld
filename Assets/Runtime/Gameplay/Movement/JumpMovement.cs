using UnityEngine;

namespace Gameplay.Animals
{
    public class JumpMovement : IAnimalMovement
    {
        private readonly float _jumpInterval;
        private readonly float _jumpForce;
        private readonly float _forwardForce;
        private readonly IAnimalBody _body;

        private float _timeToJump;

        public JumpMovement(float jumpInterval, float jumpForce, float forwardForce, IAnimalBody body)
        {
            _jumpInterval = jumpInterval;
            _jumpForce = jumpForce;
            _forwardForce = forwardForce;
            _body = body;
        }

        public void FixedTick(float deltaTime)
        {
            _timeToJump -= deltaTime;

            if (_timeToJump > 0f)
            {
                return;
            }

            Jump();

            _timeToJump = _jumpInterval;
        }

        private void Jump()
        {
            Vector3 jumpVector = Vector3.up * _jumpForce + _body.Forward * _forwardForce;
            _body.AddForce(jumpVector, ForceMode.Impulse);
        }
    }
}