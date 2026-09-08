using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer.Unity;

public class Game : IStartable
{
    private AnimalSpawner _spawner;
    private Location _location;
    private CancellationTokenSource cancellationTokenSource;

    public Game(AnimalSpawner spawner, Location location)
    {
        _spawner = spawner;
        _location = location;
        cancellationTokenSource = new CancellationTokenSource();
    }

    public void Start()
    {
        _spawner.SpawnAsync(cancellationTokenSource.Token).Forget();
    }
}
