using System;

namespace Gameplay.Game
{
    public class GameStatistics
    {
        public int DeadPreyCount { get; private set; }
        public int DeadPredatorCount { get; private set; }

        public event Action Changed;

        public void RegisterPreyDeath()
        {
            DeadPreyCount++;
            Changed?.Invoke();
        }

        public void RegisterPredatorDeath()
        {
            DeadPredatorCount++;
            Changed?.Invoke();
        }
    }
}