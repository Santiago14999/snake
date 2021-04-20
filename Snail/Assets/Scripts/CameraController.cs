using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _followSpeed;

    private float _zPosition;

    private void Start()
    {
        _zPosition = transform.position.z;
    }

    private void LateUpdate()
    {
        Vector3 position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _followSpeed);
        position.z = _zPosition;
        transform.position = position;
    }
}
