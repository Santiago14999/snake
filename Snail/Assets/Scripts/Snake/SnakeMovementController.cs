using UnityEngine;

public class SnakeMovementController : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _speedMultiplier = 2;
    [SerializeField] private float _turnSpeed = 200;
    [SerializeField] private SnakeMovementAgent _agent;

    //[SerializeField] private SnakeMovementAgent agent;
    //interface IControllable { Vector2 GetDesiredDirection(); }
    //

    private Vector3 _currentDirection;
    private float _currentSpeed;

    public float Speed { get => _currentSpeed; }

    private void Start()
    {
        _currentDirection = Vector2.right;
    }

    private void Update()
    {
        
        Vector2 desiredDirection = _agent.GetDirection(this);
     
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
