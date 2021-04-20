using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private Food _foodPrefab;
    [SerializeField] private TextureGeneratorSettings _settings;

    private SnakeMovementController _snake;

    private static FoodSpawner _instance;
    public static FoodSpawner Instance { get => _instance; }

    private void Awake()
    {
        _instance = this;
        _snake = FindObjectOfType<SnakeMovementController>();
        SpawnFood();
    }

    public void SpawnFood()
    {
        Vector2 snakePosition = new Vector2(_snake.transform.position.x, _snake.transform.position.y);
        Vector2 foodPosition = snakePosition + Random.insideUnitCircle * 8f;

        Texture2D texture = TextureGenerator.GenerateTexture(_settings);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * .5f);
        TextureGenerator.SetRandomLevelColors(_settings.colors);
        _settings.seed = Random.Range(0, int.MaxValue);

        Instantiate(_foodPrefab, foodPosition, Quaternion.identity).Sprite = sprite;
    }
}
