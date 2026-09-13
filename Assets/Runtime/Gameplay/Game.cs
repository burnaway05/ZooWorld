using System.Threading;
using VContainer.Unity;

public class Game : IStartable, ITickable, IFixedTickable
{
    private Location _location;
    private CancellationTokenSource _cancellationTokenSource;

    public Game(Location location)
    {
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
            var animal = await _location.SpawnAsync(_location.GetSpawnPosition(), _location.GetSpawnRotatin(), _cancellationTokenSource.Token);
            _location.AddAnimal(animal);
        }
    }
    public void FixedTick()
    {
        _location.FixedTick();
    }
}
