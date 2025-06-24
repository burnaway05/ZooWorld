using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Config", menuName = "Configs/AnimalConfig")]
public class AnimalConfig : ScriptableObject
{
    public AssetReference View;
}