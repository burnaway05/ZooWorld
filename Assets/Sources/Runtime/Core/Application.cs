using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Runtime.Core
{
    public class Run : MonoBehaviour
    {
        [SerializeField]
        private GameConfig _gameConfig;

        [SerializeField]
        private GameObject _locationView;

        private AssetService _assetService;
        private Location _location;

        private async void Start()
        {
            _assetService = new AssetService(_gameConfig);
            await _assetService.WarmUpPrefabs();

            _location = new Location(_locationView, _gameConfig, _assetService);
        }
    }
}