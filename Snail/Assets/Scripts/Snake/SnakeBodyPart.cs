using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyPart : MonoBehaviour
{
    [SerializeField] private bool _isHead;
    [SerializeField] private int _partsDelay;
    [HideInInspector] public SnakeBodyPart prevPart;

    public Queue<Vector2> PreviousPositions { get; private set; }

    private void Start()
    {
        PreviousPositions = new Queue<Vector2>();
    }

    private void LateUpdate()
    {
        if (_isHead)
        {
            PreviousPositions.Enqueue(transform.position);
            if (prevPart && PreviousPositions.Count == _partsDelay)
                prevPart.UpdatePart(PreviousPositions.Dequeue());

            if (PreviousPositions.Count > _partsDelay)
                PreviousPositions.Dequeue();
        }
        
    }

    public void UpdatePart(Vector2 position)
    {
        PreviousPositions.Enqueue(transform.position);
        transform.position = position;
        if (prevPart)
            prevPart.UpdatePart(PreviousPositions.Dequeue());

        if (PreviousPositions.Count > _partsDelay)
            PreviousPositions.Dequeue();
    }
}
