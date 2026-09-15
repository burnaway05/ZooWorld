using Gameplay.Animals;
using Gameplay.Definitions;
using Gameplay.Game;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZooWorld.Pooling;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField]
    private GameDefinition _gameDefinition;

    [SerializeField] 
    private LocationView _locationView;

    [SerializeField]
    private StatisticsView _statisticsView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_gameDefinition);
        builder.RegisterInstance(_locationView);
        builder.RegisterInstance(_statisticsView);

        builder.Register<GameStatistics>(Lifetime.Singleton);
        builder.Register<Location>(Lifetime.Singleton);
        builder.Register<ObjectPool>(Lifetime.Singleton);
        builder.Register<AnimalFactory>(Lifetime.Singleton);

        builder.RegisterEntryPoint<Game>();
        builder.RegisterEntryPoint<StatisticsPresenter>();
    }
}
