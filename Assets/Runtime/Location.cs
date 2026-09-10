using System.Collections.Generic;
using UnityEngine;

public class Location
{
    private List<Animal> _animals;
    private AnimalFactory _factory;
    private LocationView _view;

    public Location(LocationView view, AnimalFactory factory)
    {
        _animals = new List<Animal>();
        _factory = factory;
        _view = view;
    }

    public void AddAnimal(Animal animal)
    {
        _animals.Add(animal);
    }


    public Vector3 GetSpawnPosition()
    {
        var position = new Vector3(
               Random.Range(_view.Bounds.min.x, _view.Bounds.max.x),
               _view.Bounds.max.y,
               Random.Range(_view.Bounds.min.z, _view.Bounds.max.z));

        return position;
    }

    public Quaternion GetSpawnRotatin()
    {
        return Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
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
