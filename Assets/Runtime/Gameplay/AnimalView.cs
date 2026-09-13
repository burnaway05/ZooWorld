using UnityEngine;

namespace Gameplay.Animals
{
    public interface IAnimalBody
    {
        Rigidbody Rigidbody { get; }
        Vector3 Position { get; }
        Vector3 Forward { get; }
    }

    public interface IAnimalCollisionHandler
    {
        void HandleCollision(IAnimalBody first, IAnimalBody second);
    }

    public class AnimalView : MonoBehaviour, IAnimalBody
    {
        [SerializeField]
        private Rigidbody _rigidbody;

        private IAnimalCollisionHandler _collisionHandler;

        public Rigidbody Rigidbody => _rigidbody;

        public Vector3 Position => transform.localPosition;

        public Vector3 Forward => transform.forward;

        public void Bind(IAnimalCollisionHandler collisionHandler)
        {
            _collisionHandler = collisionHandler;
        }

        public void Unbind()
        {
            _collisionHandler = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<AnimalView>(out var other))
            {
                _collisionHandler?.HandleCollision(this, other);
            }
        }
    }
}