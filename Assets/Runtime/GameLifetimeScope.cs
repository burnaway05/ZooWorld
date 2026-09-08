using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField]
    private GameDefinition _gameDefinition;

    [SerializeField] 
    private Location _location;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_gameDefinition);
        //builder.RegisterInstance(_location);
        builder.Register<Location>(Lifetime.Singleton);
        builder.Register<AnimalSpawner>(Lifetime.Singleton);

        builder.RegisterEntryPoint<Game>();
    }
}
