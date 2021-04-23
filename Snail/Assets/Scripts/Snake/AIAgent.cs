using UnityEngine;

[CreateAssetMenu(fileName = "AI Agent", menuName = "ScriptableObjects/AIAgent")]
public class AIAgent : SnakeMovementAgent
{
    [SerializeField] private float _wanderStrength = 1f;
    private Vector2 desiredDirection = Vector2.right;

    public override Vector2 GetDirection(SnakeMovementController snake)
    {
        desiredDirection = (desiredDirection + Random.insideUnitCircle * _wanderStrength).normalized;
        return  desiredDirection;
    }
}
