using Cysharp.Threading.Tasks;
using System;
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

        [SerializeField]
        private GameObject _tastyLabel;
        
        private float _tastyOffset = 1.2f;

        private IAnimalCollisionHandler _collisionHandler;
        private Animal _animal;

        public Rigidbody Rigidbody => _rigidbody;

        public Vector3 Position => transform.localPosition;

        public Vector3 Forward => transform.forward;

        public void Bind(Animal animal, IAnimalCollisionHandler collisionHandler)
        {
            _animal = animal;
            _animal.Ate += OnAte;
            _collisionHandler = collisionHandler;

            if (_tastyLabel != null)
            {
                _tastyLabel.SetActive(false);
            }
        }

        public void Unbind()
        {
            _animal = null;
            _collisionHandler = null;

            if (_tastyLabel != null)
            {
                _tastyLabel.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<AnimalView>(out var other))
            {
                _collisionHandler?.HandleCollision(this, other);
            }
        }

        private void LateUpdate()
        {
            if(_tastyLabel != null)
            {
                Vector3 screenDown = -Camera.main.transform.up;
                screenDown = Vector3.ProjectOnPlane(screenDown, Vector3.up).normalized;

                _tastyLabel.transform.position = transform.position + screenDown * _tastyOffset;
                _tastyLabel.transform.rotation = Camera.main.transform.rotation;
            }
        }

        private void OnAte()
        {
            ShowTastyAsync().Forget();
        }

        private async UniTask ShowTastyAsync()
        {
            _tastyLabel?.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            _tastyLabel?.SetActive(false);
        }
    }
}