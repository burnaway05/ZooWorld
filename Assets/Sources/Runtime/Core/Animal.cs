using UnityEngine;

namespace Assets.Sources.Runtime.Core
{
    internal abstract class Animal
    {
        public abstract void OnCollisionEnter(Collision other);
    }
}