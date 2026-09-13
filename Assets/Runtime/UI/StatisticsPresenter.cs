using Gameplay.Game;
using System;
using VContainer.Unity;

namespace UI
{
    public class StatisticsPresenter : IStartable, IDisposable
    {
        private readonly GameStatistics _statistics;
        private readonly StatisticsView _view;

        public StatisticsPresenter(GameStatistics statistics, StatisticsView view)
        {
            _statistics = statistics;
            _view = view;
        }

        public void Start()
        {
            _statistics.Changed += Refresh;
            Refresh();
        }

        private void Refresh()
        {
            _view.SetDeadPrey(_statistics.DeadPreyCount);
            _view.SetDeadPredators(_statistics.DeadPredatorCount);
        }

        public void Dispose()
        {
            _statistics.Changed -= Refresh;
        }
    }
}