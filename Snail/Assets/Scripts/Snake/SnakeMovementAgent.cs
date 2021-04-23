using UnityEngine;

public abstract class SnakeMovementAgent : ScriptableObject
{
    public abstract Vector2 GetDirection(SnakeMovementController snake);
}