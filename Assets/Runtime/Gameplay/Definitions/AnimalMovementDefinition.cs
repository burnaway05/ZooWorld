using UnityEngine;

public abstract class AnimalMovementDefinition : ScriptableObject
{
    public abstract IAnimalMovement Create(IAnimalBody body);
}
