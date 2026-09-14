using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Gameplay
{

    public class AnimalNotificationView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _tastyLabel;

        [SerializeField]
        private float _duration = 1f;

        private CancellationTokenSource _cts;

        public void ShowTasty()
        {
            ShowTastyAsync().Forget();
        }

        private async UniTaskVoid ShowTastyAsync()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _cts = new CancellationTokenSource();

            _tastyLabel.SetActive(true);

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_duration), cancellationToken: _cts.Token);
            }
            catch (OperationCanceledException)
            {
            }

            if (!_cts.IsCancellationRequested)
            {
                _tastyLabel.SetActive(false);
            }
        }
    }
}