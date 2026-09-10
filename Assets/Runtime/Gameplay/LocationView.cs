using UnityEngine;

public class LocationView : MonoBehaviour
{
    [SerializeField]
    private MeshCollider _collider;

    public Bounds Bounds => _collider.bounds;
}
