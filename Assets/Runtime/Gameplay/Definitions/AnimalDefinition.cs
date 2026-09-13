using Gameplay.Animals;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Definitions
{
    [CreateAssetMenu(fileName = "Definition", menuName = "Definitions/AnimalDefinition")]
    public class AnimalDefinition : ScriptableObject
    {
        public string Name;
        public AnimalType Type;
        public AssetReference Prefab;
        public AnimalMovementDefinition Movement;

        public IAnimalMovement CreateMovement(IAnimalBody body)
        {
            return Movement.Create(body);
        }
    }

    public enum AnimalType
    {
        Prey,
        Predator
    }
}