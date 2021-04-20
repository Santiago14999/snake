using UnityEngine;

public class SnakeMovementController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _turnSpeed;

    private Vector3 _currentDirection;
    private float _currentSpeed;

    private void Start()
    {
        _currentDirection = Vector2.right;
    }

    private void Update()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredDirection = (target - (Vector2)transform.position).normalized;
        if (Vector2.Distance(_currentDirection, desiredDirection) != 0f)
        {
            float rotationDirection = Vector2.SignedAngle(_currentDirection, desiredDirection) > 0 ? 1f : -1f;
            _currentDirection = Quaternion.Euler(0, 0, _turnSpeed * Time.deltaTime * rotationDirection) * _currentDirection;
        }

        _currentSpeed = Input.GetButton("Fire1") ? _speed * _speedMultiplier : _speed;
        transform.position += _currentDirection * Time.deltaTime * _currentSpeed;
        transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(_currentDirection.y, _currentDirection.x) * Mathf.Rad2Deg);
    }
}
