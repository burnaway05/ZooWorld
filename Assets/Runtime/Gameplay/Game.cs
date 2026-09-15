using Cysharp.Threading.Tasks;
using Gameplay.Definitions;
using System;
using System.Threading;
using VContainer.Unity;
using ZooWorld.Pooling;

namespace Gameplay.Game
{
    public class Game : IStartable, ITickable, IFixedTickable, IDisposable
    {
        private const int poolWwarmUpCount = 10;
        private readonly GameDefinition _gameDefinition;
        private readonly ObjectPool _objectPool;
        private readonly Location _location;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public Game(GameDefinition gameDefinition, ObjectPool objectPool, Location location)
        {
            _gameDefinition = gameDefinition;
            _objectPool = objectPool;
            _location = location;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            RunAsync().Forget();
        }

        private async UniTaskVoid RunAsync()
        {
            foreach (var animal in _gameDefinition.Animals)
            {
                await _objectPool.WarmUpAsync(animal.Prefab, poolWwarmUpCount, _cancellationTokenSource.Token);
            }

            await SpawnAnimalsAsync();
        }

        public void Tick()
        {
            _location.Tick();
        }

        private async UniTask SpawnAnimalsAsync()
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

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}