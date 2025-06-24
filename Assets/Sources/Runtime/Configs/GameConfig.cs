using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    public AnimalConfig[] AnimalsConfig;
    public int SpawnAnimalInterval = 2;
}
