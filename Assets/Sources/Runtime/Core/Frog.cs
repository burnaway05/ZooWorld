using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Runtime.Core
{
    internal class Frog : Animal
    {
        private GameObject _view;
        public float jumpForce = 5f;
        public float forwardForce = 3f;
        public float jumpInterval = 2f;
        private Rigidbody rb;
        private bool canJump = true;

        public Frog(GameObject view)
        {
            _view = view;
            _view.GetComponent<AnimalView>().Initialize(this);
            rb = _view.GetComponent<Rigidbody>();
            TryJump();
        }

        private async void TryJump()
        {
            while (true)
            {
                if (canJump)
                {
                    Vector3 jumpVector = Vector3.up * jumpForce + _view.transform.forward * forwardForce;
                    rb.AddForce(jumpVector, ForceMode.Impulse);
                    await UniTask.Delay(System.TimeSpan.FromSeconds(jumpInterval));
                }
            }
        }

        public override void OnCollisionEnter(Collision other)
        {
            canJump = true;
        }
    }
}
