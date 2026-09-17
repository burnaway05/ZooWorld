using Gameplay.Animals;
using UnityEngine;

namespace Gameplay.Definitions
{
    [CreateAssetMenu(fileName = "LinearAnimalMovementDefinition", menuName = "Definitions/LinearDefinition")]
    public class LinearAnimalMovementDefinition : AnimalMovementDefinition
    {
        [SerializeField]
        private float _speed = 1f;

        public override IAnimalMovement Create(IAnimalBody body)
        {
            return new LinearMovement(_speed, body);
        }
    }
}