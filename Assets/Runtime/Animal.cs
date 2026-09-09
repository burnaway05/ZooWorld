using UnityEngine;

public class Animal
{
    public bool IsAlive { get; set; }
    
    public AnimalView View { get; private set; }
    public AnimalDefinition Definition { get; private set; }


    public Animal(AnimalDefinition definition, AnimalView view)
    {
        Definition = definition;
        View = view;
    }
}
