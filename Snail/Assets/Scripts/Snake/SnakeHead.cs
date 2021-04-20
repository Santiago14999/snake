using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public float followDistance;
    public Vector2 lastPosition;
    [SerializeField] private SnakeBodyPartLerp _bodyPart;
    [SerializeField] private int _startBodyPartsAmount;

    private Transform _tail;
    private SnakeSpriteGenerator _generator;
    private int _bodyPartsCount;
    private List<Vector2> _positions;

    private void Start()
    {
        _positions = new List<Vector2>();
        if (TryGetComponent(out _generator))
        {
            InitializeSnakeGenerator();
        }
        
        for (int i = 0; i < _startBodyPartsAmount; i++)
        {
            AddBodyPart();
        }
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, lastPosition) >= followDistance)
        {
            lastPosition = transform.position;
            _positions.Insert(0, lastPosition);
            if (_positions.Count > _bodyPartsCount)
                _positions.RemoveAt(_positions.Count - 1);
        }
    }

    public Vector2 GetPositionAt(int index)
    {
        return _positions[index];
    }

    private void InitializeSnakeGenerator()
    {
        _generator.InitializeSnake();
        GetComponentInChildren<SpriteRenderer>().sprite = _generator.GenerateHead();
    }

    public void AddBodyPart()
    {
        Vector2 position = _tail == null ? transform.position : _tail.position;
        SnakeBodyPartLerp part = Instantiate(_bodyPart, position, Quaternion.identity);
        part.nextPart = _tail == null ? transform : _tail;
        part.head = this;
        part.index = _bodyPartsCount;
        _bodyPartsCount++;
        _positions.Add(_tail == null ? transform.position : _tail.position);

        if (_generator)
        {
            if (_tail)
            {
                part.GetComponent<SpriteRenderer>().sprite = _generator.GenerateTail(_bodyPartsCount);
                _tail.GetComponent<SpriteRenderer>().sprite = _generator.GenerateBody(_bodyPartsCount - 1);
            }
            else
            {
                part.GetComponent<SpriteRenderer>().sprite = _generator.GenerateTail(_bodyPartsCount);
            }
        }

        _tail = part.transform;
    }
}
