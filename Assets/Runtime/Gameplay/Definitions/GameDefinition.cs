using UnityEngine;

namespace Gameplay.Definitions
{
    [CreateAssetMenu(fileName = "GameDefinition", menuName = "Definitions/GameDefinition")]
    public class GameDefinition : ScriptableObject
    {
        public AnimalDefinition[] Animals;
        public float MinSpawnnterval = 1;
        public float MaxSpawnnterval = 2;
    }
}