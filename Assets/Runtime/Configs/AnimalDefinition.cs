using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Definition", menuName = "Definitions/AnimalDefinition")]
public class AnimalDefinition : ScriptableObject
{
    public string Name;
    public AssetReference View;
}