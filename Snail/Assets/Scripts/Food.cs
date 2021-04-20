using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    public Sprite Sprite { set => _renderer.sprite = value; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<SnakeHead>(out var snake))
        {
            Destroy(gameObject);
            snake.AddBodyPart();
            FoodSpawner.Instance.SpawnFood();
        }
    }
}
