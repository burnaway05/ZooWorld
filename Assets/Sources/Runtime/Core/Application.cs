using UnityEngine;

public class Run : MonoBehaviour
{
    [SerializeField] 
    private GameConfig _gameConfig;

    private Location _location;

    private void Start()
    {
        _location = new Location(_gameConfig);
        _location.SpawnAnimal();
    }
}