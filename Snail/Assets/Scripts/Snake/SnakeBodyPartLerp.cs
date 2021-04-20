using UnityEngine;

public class SnakeBodyPartLerp : MonoBehaviour
{
    [HideInInspector] public SnakeHead head;
    [HideInInspector] public Transform nextPart;
    [HideInInspector] public int index;

    private void Update()
    {
        //Vector2 direction = (transform.position - nextPart.position).normalized * head.followDistance;
        //transform.position = (Vector2)nextPart.position + direction;
        //transform.rotation = Quaternion.Euler(0, 0, 180f + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        Vector2 direction = transform.position - nextPart.position;
        transform.position = Vector2.MoveTowards(transform.position, head.GetPositionAt(index), Time.deltaTime * 10f * direction.magnitude);
        transform.rotation = Quaternion.Euler(0, 0, 180f + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }
}
