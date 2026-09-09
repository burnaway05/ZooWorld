using System.Collections.Generic;
using VContainer.Unity;

public class Location
{
    private List<Animal> _animals;
    private AnimalFactory _factory;

    public Location(AnimalFactory factory)
    {
        _animals = new List<Animal>();
        _factory = factory;
    }

    public void AddAnimal(Animal animal)
    {
        _animals.Add(animal);
    }

    public void Tick()
    {
        for (var i = _animals.Count - 1; i >= 0; i--)
        {
            if (!_animals[i].IsAlive)
            {
                _factory.Release(_animals[i]);
                _animals.RemoveAt(i);
            }
        }
    }
}
