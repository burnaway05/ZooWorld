using UnityEngine;

namespace Assets.Sources.Runtime.Core
{
    internal abstract class Animal
    {
        public virtual void FixedUpdate() { }

        public virtual void OnCollisionEnter(Collision other) { }
    }
}