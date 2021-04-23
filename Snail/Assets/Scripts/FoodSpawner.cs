using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private Food _foodPrefab;
    [SerializeField] private TextureGeneratorSettings _settings;
    [SerializeField] private Vector2 _mapSize;
    [SerializeField] private float _respawnTime;

    private SnakeMovementController _snake;

    private static FoodSpawner _instance;
    public static FoodSpawner Instance { get => _instance; }

    private void Awake()
    {
        _instance = this;
        _snake = FindObjectOfType<SnakeMovementController>();
        for (float y = -_mapSize.y; y < _mapSize.y; y += 10)
        {
            for (float x = -_mapSize.x; x < _mapSize.x; x += 10)
            {
                Vector2 center = new Vector2(x + 5f, y + 5f);
                Vector2 direction = Random.insideUnitCircle;
                for (int i = 0; i < 3; i++)
                {
                    SpawnFood(center + direction * Random.Range(1f, 6f));
                    direction = Quaternion.Euler(0, 0, 120f) * direction;
                }
            }
        }
    }

    public void SpawnFood(Vector2 position)
    {
        Texture2D texture = TextureGenerator.GenerateTexture(_settings);
        _settings.seed = Random.Range(0, int.MaxValue);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * .5f);
        TextureGenerator.SetRandomLevelColors(_settings.colors);

        Instantiate(_foodPrefab, position, Quaternion.Euler(0, 0, Random.Range(0, 360f))).Sprite = sprite;
    }
}
