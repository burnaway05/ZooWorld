using UnityEngine;

namespace Gameplay.Animals
{
    public interface IAnimalMovement
    {
        void FixedTick(float deltaTime);
    }
}