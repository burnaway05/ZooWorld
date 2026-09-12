using UnityEngine;

[CreateAssetMenu(fileName = "JumpAnimalMovementDefinition", menuName = "Definitions/JumpDefinition")]
public class JumpAnimalMovementDefinition : AnimalMovementDefinition
{
    [SerializeField]
    private float _jumpInterval = 2;

    [SerializeField]
    private float _jumpForce = 5;

    [SerializeField]
    private float _forwardForce = 3;

    public override IAnimalMovement Create(IAnimalBody body)
    {
        return new JumpMovement(_jumpInterval, _jumpForce, _forwardForce, body);
    }
}
