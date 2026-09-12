using UnityEngine;

public interface IAnimalBody
{
    Rigidbody Rigidbody { get; }
    Vector3 Position { get; }
    Vector3 Forward { get; }
}

public class AnimalView : MonoBehaviour, IAnimalBody
{
    [SerializeField]
    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody => _rigidbody;

    public Vector3 Position => transform.localPosition;

    public Vector3 Forward => transform.forward;
}
