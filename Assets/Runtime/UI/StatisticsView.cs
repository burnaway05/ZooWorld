using TMPro;
using UnityEngine;
using ZooWorld.Pooling;

namespace ZooWorld.UI
{
    public class StatisticsView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _deadPreyText;

        [SerializeField]
        private TMP_Text _deadPredatorsText;

        [SerializeField]
        private TMP_Text _status;

        public void SetDeadPrey(int value)
        {
            _deadPreyText.text = $"Dead prey: {value}";
        }

        public void SetDeadPredators(int value)
        {
            _deadPredatorsText.text = $"Dead predators: {value}";
        }
    }
}