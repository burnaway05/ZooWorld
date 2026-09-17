using Gameplay.Animals;
using UnityEngine;

namespace Gameplay.Definitions
{
    public abstract class AnimalMovementDefinition : ScriptableObject
    {
        public abstract IAnimalMovement Create(IAnimalBody body);
    }
}