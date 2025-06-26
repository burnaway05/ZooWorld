using UnityEngine;

namespace Assets.Sources.Runtime.Core
{
    internal class AnimalView : MonoBehaviour
    {
        private Animal _animal;

        public void Initialize(Animal animal)
        {
            _animal = animal;
        }

        private void FixedUpdate()
        {
            _animal.FixedUpdate();
        }

        private void OnCollisionEnter(Collision other)
        {
            _animal.OnCollisionEnter(other);
        }
    }
}