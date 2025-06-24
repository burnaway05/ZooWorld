using UnityEngine;

public class Run : MonoBehaviour
{
    [SerializeField]
    private GameConfig _gameConfig;

    [SerializeField] 
    private GameObject _locationView;

    private Location _location;

    private void Start()
    {
        _location = new Location(_locationView, _gameConfig);
    }
}