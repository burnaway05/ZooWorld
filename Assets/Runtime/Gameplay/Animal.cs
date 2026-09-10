using UnityEngine;

public class Animal
{
    public AnimalDefinition Definition { get; private set; }
    public AnimalView View { get; private set; }
    public bool IsAlive { get; set; }

    public Animal(AnimalDefinition definition, AnimalView view)
    {
        Definition = definition;
        View = view;
        IsAlive = true;
    }
}
