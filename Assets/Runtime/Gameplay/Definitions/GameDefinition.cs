using UnityEngine;

namespace Gameplay.Definitions
{
    [CreateAssetMenu(fileName = "GameDefinition", menuName = "Definitions/GameDefinition")]
    public class GameDefinition : ScriptableObject
    {
        public AnimalDefinition[] Animals;
        public int MinSpawnnterval = 1;
        public int MaxSpawnnterval = 2;
    }
}