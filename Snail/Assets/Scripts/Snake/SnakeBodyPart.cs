using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyPart : MonoBehaviour
{
    [HideInInspector] public SnakeHead head;
    [HideInInspector] public Transform nextPart;
    [HideInInspector] public int index;

    private SnakeMovementController _movement;

    private void Start()
    {
        _movement = head.GetComponent<SnakeMovementController>();
    }

    private void Update()
    {
        Vector2 direction = transform.position - nextPart.position;
        transform.position = Vector2.MoveTowards(transform.position, head.GetPositionAt(index), Time.deltaTime * _movement.Speed * direction.magnitude);
        transform.rotation = Quaternion.Euler(0, 0, 180f + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }
}
