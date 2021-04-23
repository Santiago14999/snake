using UnityEngine;

[CreateAssetMenu(fileName = "HumanAgent", menuName = "ScriptableObjects/HumanAgent")]
public class HumanAgent : SnakeMovementAgent
{
    private Camera _camera;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();
    }

    public override Vector2 GetDirection(SnakeMovementController snake)
    {
        Vector2 target = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredDirection = (target - (Vector2)snake.transform.position).normalized;

        return desiredDirection;
    }
}
