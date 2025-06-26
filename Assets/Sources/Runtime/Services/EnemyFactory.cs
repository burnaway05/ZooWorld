using Assets.Sources.Runtime.Core;
using UnityEngine;

namespace Assets.Sources.Runtime.Services
{
    internal class EnemyFactory
    {
        private AssetService _assetService;

        public EnemyFactory(AssetService assetService)
        {
            _assetService = assetService;
        }

        public Animal Create(string enemy, Vector3 position, Quaternion rotation)
        {
            var view = _assetService.Get(enemy);
            view.transform.localPosition = position;
            view.transform.localRotation = rotation;

            Animal animal = default;
            switch (enemy)
            {
                case "Frog":
                    animal = new Frog(view);
                    break;

                case "Snake":
                    animal = new Snake(view);
                    break;
                
            }

            return animal;
        }
    }
}
