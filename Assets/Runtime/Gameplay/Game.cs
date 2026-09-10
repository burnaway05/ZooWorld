using System.Threading;
using VContainer.Unity;

public class Game : IStartable, ITickable
{
    private AnimalSpawner _spawner;
    private Location _location;
    private CancellationTokenSource _cancellationTokenSource;

    public Game(AnimalSpawner spawner, Location location)
    {
        _spawner = spawner;
        _location = location;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Start()
    {
        SpawnAnimals();
    }

    public void Tick()
    {
        _location.Tick();
    }

    private async void SpawnAnimals()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            var animal = await _spawner.SpawnAsync(_location.GetSpawnPosition(), _location.GetSpawnRotatin(), _cancellationTokenSource.Token);
            _location.AddAnimal(animal);
        }
    }
}
