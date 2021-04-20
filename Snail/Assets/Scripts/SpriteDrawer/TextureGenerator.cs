using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct ColorLevel
    {
        public Color color;
        [Range(-1f, 1f)] public float level;
    }

    [SerializeField] private Renderer _renderer;
    [SerializeField] private int _width = 64;
    [SerializeField] private int _height = 64;
    [SerializeField] private int _seed = 0;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Vector2 _grayscaleRange = Vector2.one;
    [SerializeField] private float _perlinScale = 10f;
    [SerializeField] private bool _randomColors;
    [SerializeField] private ColorLevel[] _colors;
    [SerializeField] private bool _pointFilter;
    [SerializeField] private Texture2D _overlayTexture;
    [Header("Outline")]
    [SerializeField] private bool _outline;
    [SerializeField] private bool _leftOutline;
    [SerializeField] private bool _rightOutline;
    [SerializeField] private ColorLevel[] _outlineColors;
    [Header("Border Radius")]
    [SerializeField] private bool _leftRadius;
    [SerializeField] private bool _rightRadius;
    [SerializeField] private int _borderRadius;
    [SerializeField] private bool _autoUpdate;
    [SerializeField] private TextureGeneratorSettings _preset;

    private Texture2D _currentTexture;
    public Texture2D CurrentTexture { get => _currentTexture; }
    public bool AutoUpdate { get => _autoUpdate; }

    public TextureGeneratorSettings GetSettings()
    {
        TextureGeneratorSettings settings = ScriptableObject.CreateInstance<TextureGeneratorSettings>();
        settings.borderRadius = _borderRadius;
        settings.colors = _colors;
        settings.grayscale = _grayscaleRange;
        settings.height = _height;
        settings.leftRadius = _leftRadius;
        settings.rightRadius = _rightRadius;
        settings.outline = _outline;
        settings.leftOutline = _leftOutline;
        settings.rightOutline = _rightOutline;
        settings.outlineColors = _outlineColors;
        settings.perlinScale = _perlinScale;
        settings.pointFilter = _pointFilter;
        settings.seed = _seed;
        settings.width = _width;
        settings.overlayTexture = _overlayTexture;
        return settings;
    }

    public void GeneratePreset()
    {
        if (!_preset)
        {
            Debug.LogError("Preset is not set");
            return;
        }    
        Texture2D texture = GenerateTexture(_preset);
        _currentTexture = texture;
        if (!_renderer)
            _renderer = GetComponent<MeshRenderer>();
        _renderer.sharedMaterial.mainTexture = texture;
    }

    public static Texture2D GenerateTexture(TextureGeneratorSettings settings)
    {
        Texture2D texture = new Texture2D(settings.width, settings.height);
        Color[] colorMap = new Color[settings.width * settings.height];
        float[,] perlinMap = GetPerlinNoiseMap(settings.width, settings.height, settings.seed, settings.perlinScale, settings.offset);

        Color[] borderdMask = GetRoundMask(settings.borderRadius);
        for (int y = 0; y < settings.height; y++)
        {
            for (int x = 0; x < settings.width; x++)
            {
                colorMap[y * settings.width + x] = Color.white;
                if (settings.borderRadius > 1 && IsOutsideBorderRadius(borderdMask, x, y, settings.width, settings.height, settings.borderRadius, settings.leftRadius, settings.rightRadius))
                {
                    colorMap[y * settings.width + x] = Color.clear;
                }

                Color color = Color.white;
                if (settings.outline)
                {
                    if (IsOutlinePixel(borderdMask, colorMap, x, y, settings.width, settings.height, settings.borderRadius, settings.leftOutline, settings.rightOutline, settings.leftRadius, settings.rightRadius))
                    {
                        foreach (var c in settings.outlineColors)
                            if (perlinMap[x, y] >= c.level)
                                color = c.color;

                        color *= Random.Range(settings.grayscale.x, settings.grayscale.y);
                        colorMap[y * settings.width + x] = color;
                        continue;
                    }
                }
                if (colorMap[y * settings.width + x] == Color.clear)
                    continue;

                if (settings.colors.Length > 0)
                {
                    foreach (var c in settings.colors)
                        if (perlinMap[x, y] >= c.level)
                            color = c.color;
                }

                color *= Random.Range(settings.grayscale.x, settings.grayscale.y);
                colorMap[y * settings.width + x] = color;
            }
        }

        if (settings.overlayTexture)
            ApplyOverlayTexture(colorMap, settings.width, settings.height, settings.overlayTexture);

        texture.filterMode = settings.pointFilter ? FilterMode.Point : FilterMode.Bilinear;
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }

    private static void ApplyOverlayTexture(Color[] map, int width, int height, Texture2D texture)
    {
        if (width != texture.width || height != texture.height)
            return;

        Color[] textureMap = texture.GetPixels();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (textureMap[y * width + x].a != 0)
                    map[y * width + x] = textureMap[y * width + x];
            }
        }
    }

    public static void SetRandomLevelColors(ColorLevel[] colors)
    {
        float currentColor = Random.Range(0f, 1f);
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].color = Color.HSVToRGB(currentColor, 1f, 1f);
            currentColor += .2f;
            currentColor %= 1f;
        }
    }

    public void ApplyTexture()
    {
        if (_randomColors)
            SetRandomLevelColors(_colors);

        Texture2D texture = new Texture2D(_width, _height);
        Color[] colorMap = new Color[_width * _height];
        float[,] perlinMap = GetPerlinNoiseMap(_width, _height, _seed, _perlinScale, _offset);

        Color[] borderdMask = GetRoundMask(_borderRadius);
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                colorMap[y * _width + x] = Color.white;
                if (_borderRadius > 1 && IsOutsideBorderRadius(borderdMask, x, y, _width, _height, _borderRadius, _leftRadius, _rightRadius))
                {
                    colorMap[y * _width + x] = Color.clear;
                }

                Color color = Color.white;
                if (_outline)
                {
                    if (IsOutlinePixel(borderdMask, colorMap, x, y, _width, _height, _borderRadius, _leftOutline, _rightOutline, _leftRadius, _rightRadius))
                    {
                        foreach (var c in _outlineColors)
                            if (perlinMap[x, y] >= c.level)
                                color = c.color;

                        color *= Random.Range(_grayscaleRange.x, _grayscaleRange.y);
                        colorMap[y * _width + x] = color;
                        continue;
                    }
                }
                if (colorMap[y * _width + x] == Color.clear)
                    continue;

                if (_colors.Length > 0)
                {
                    foreach (var c in _colors)
                        if (perlinMap[x, y] >= c.level)
                            color = c.color;
                }
                
                color *= Random.Range(_grayscaleRange.x, _grayscaleRange.y);
                colorMap[y * _width + x] = color;
            }
        }

        if (_overlayTexture)
            ApplyOverlayTexture(colorMap, _width, _height, _overlayTexture);
        texture.filterMode = _pointFilter ? FilterMode.Point : FilterMode.Bilinear;
        texture.SetPixels(colorMap);
        texture.Apply();

        _currentTexture = texture;
        if (!_renderer)
            _renderer = GetComponent<MeshRenderer>();
        _renderer.sharedMaterial.mainTexture = texture;
    }

    private static bool IsOutlinePixel(Color[] mask, Color[] map, int x, int y, int width, int height, int borderRadius, bool left, bool right, bool leftRadius, bool rightRadius)
    {
        if (map[y * width + x] != Color.clear)
        {
            if (y == 0 || y == height - 1)
                return true;

            if (left && x == 0)
                return true;

            if (right && x == width - 1)
                return true;

            if (map[(y - 1) * width + x] == Color.clear)
                return true;

            if (map[y * width + x - 1] == Color.clear)
                return true;

            if (IsOutsideBorderRadius(mask, x + 1, y, width, height, borderRadius, leftRadius, rightRadius))
                return true;

            if (IsOutsideBorderRadius(mask, x, y + 1, width, height, borderRadius, leftRadius, rightRadius))
                return true;
        }

        return false;
    }

    private static bool IsOutsideBorderRadius(Color[] circleMap, int x, int y, int width, int height, int borderRadius, bool left, bool right)
    {
        if (borderRadius < 1)
            return false;

        if (!left && x < borderRadius || !right && x >= width - borderRadius)
            return false;

        if (x < borderRadius && y < borderRadius)
        {
            if ((circleMap[y * (borderRadius * 2 - 1) + x] == Color.black))
            {
                return true;
            }
        }
        if (x < borderRadius && y >= height - borderRadius)
        {
            if ((circleMap[Mathf.Abs(y - height + 1) * (borderRadius * 2 - 1) + x] == Color.black))
            {
                return true;
            }
        }
        if (x >= width - borderRadius && y < borderRadius)
        {
            if ((circleMap[y * (borderRadius * 2 - 1) + Mathf.Abs(x - width + 1)] == Color.black))
            {
                return true;
            }
        }
        if (x >= width - borderRadius && y >= height - borderRadius)
        {
            if ((circleMap[Mathf.Abs(y - height + 1) * (borderRadius * 2 - 1) + Mathf.Abs(x - width + 1)] == Color.black))
            {
                return true;
            }
        }

        return false;
    }

    private static Color[] GetRoundMask(int radius)
    {
        radius = Mathf.Clamp(radius, 1, int.MaxValue);
        int width = radius * 2 - 1;
        int height = radius * 2 - 1;
        Color[] map = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Mathf.Pow(x - radius + 1, 2) + Mathf.Pow(y - radius + 1, 2) <= Mathf.Pow(radius, 2))
                    map[y * width + x] = Color.white;
                else
                    map[y * width + x] = Color.black;
            }
        }
        return map;
    }

    private static float[,] GetPerlinNoiseMap(int width, int height, int seed, float scale, Vector2 offset)
    {
        System.Random prng = new System.Random(seed);
        int seedX = prng.Next(-100_000, 100_000);
        int seedY = prng.Next(-100_000, 100_000);
        float[,] map = new float[width, height];
        scale = Mathf.Clamp(scale, .001f, 1000f);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = x / scale + seedX + offset.x;
                float sampleY = y / scale + seedY + offset.y;
                float value = Mathf.PerlinNoise(sampleX, sampleY);
                map[x, y] = value;
            }
        }

        return map;
    }
}
