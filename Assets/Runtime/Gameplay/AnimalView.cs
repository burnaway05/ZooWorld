using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Gameplay.Animals
{
    public interface IAnimalBody
    {
        Vector3 Position { get; }
        Vector3 Forward { get; }
        void MoveRotation(Quaternion rotation);
        public void AddForce(Vector3 force, ForceMode mode); 
        void SetLinearVelocity(Vector3 velocity);
    }

    public interface IAnimalCollisionHandler
    {
        void HandleCollision(IAnimalBody first, IAnimalBody second);
    }

    public class AnimalView : MonoBehaviour, IAnimalBody
    {
        private const float tastyOffset = 1.2f;

        [SerializeField]
        private Rigidbody _rigidbody;

        [SerializeField]
        private GameObject _tastyLabel;
        
        private IAnimalCollisionHandler _collisionHandler;
        private Animal _animal;
        private CancellationTokenSource _cancellationTokenSource;

        public Vector3 Position => transform.position;

        public Vector3 Forward => transform.forward;

        public void Bind(Animal animal, IAnimalCollisionHandler collisionHandler)
        {
            _cancellationTokenSource = new CancellationTokenSource();

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
            if (_animal != null)
            {
                _animal.Ate -= OnAte;
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

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

                _tastyLabel.transform.position = transform.position + screenDown * tastyOffset;
                _tastyLabel.transform.rotation = Camera.main.transform.rotation;
            }
        }

        private void OnAte()
        {
            ShowTastyAsync(_cancellationTokenSource.Token).Forget();
        }

        private async UniTask ShowTastyAsync(CancellationToken token)
        {
            _tastyLabel?.SetActive(true);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);

            _tastyLabel?.SetActive(false);
        }

        public void MoveRotation(Quaternion rotation)
        {
            _rigidbody.MoveRotation(rotation);
        }

        public void AddForce(Vector3 force, ForceMode mode)
        {
            _rigidbody.AddForce(force, mode);
        }

        public void SetLinearVelocity(Vector3 velocity)
        {
            _rigidbody.linearVelocity = velocity;
        }
    }
}