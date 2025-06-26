using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Runtime.Core
{
    internal class Snake : Animal
    {
        private GameObject _view;
        public float speed = 1f;
        private Rigidbody rb;

        public Snake(GameObject view)
        {
            _view = view;
            _view.GetComponent<AnimalView>().Initialize(this);
            rb = _view.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public override void FixedUpdate()
        {
            rb.velocity = Vector3.forward * speed + Vector3.up * rb.velocity.y;
        }

        public override void OnCollisionEnter(Collision other)
        {
        }
    }
}
