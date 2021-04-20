using UnityEngine;

public class SnakeSpriteGenerator : MonoBehaviour
{
    [SerializeField] private TextureGeneratorSettings[] _generatorSettings;
    [SerializeField] private bool _progressiveColors;

    private int _snakeSeed;
    private TextureGenerator.ColorLevel[] _colors;

    public void InitializeSnake()
    {
        _snakeSeed = Random.Range(0, int.MaxValue);
        _colors = new TextureGenerator.ColorLevel[_generatorSettings[0].colors.Length];
        _generatorSettings[0].colors.CopyTo(_colors, 0);
        TextureGenerator.SetRandomLevelColors(_colors);

        for (int i = 0; i < _generatorSettings.Length; i++)
        {
            _generatorSettings[i] = _generatorSettings[i].Copy();
            _generatorSettings[i].seed = _snakeSeed;
            _generatorSettings[i].colors = _colors;
        }
    }

    public Sprite GenerateHead()
    {
        Texture2D texture = TextureGenerator.GenerateTexture(_generatorSettings[0]);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * .5f);
    }

    public Sprite GenerateBody(int index)
    {
        _generatorSettings[1].offset.x = _generatorSettings[1].width * index * 1f / _generatorSettings[1].perlinScale;

        if (_progressiveColors)
        {
            for (int i = 0; i < _generatorSettings[1].colors.Length; i++)
            {
                Color.RGBToHSV(_generatorSettings[1].colors[i].color, out var h, out var s, out var v);
                h += index / 500f;
                h %= 1f;
                _generatorSettings[1].colors[i].color = Color.HSVToRGB(h, s, v);
            }
        }
        
        Texture2D texture = TextureGenerator.GenerateTexture(_generatorSettings[1]);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * .5f);
    }

    public Sprite GenerateTail(int index)
    {
        _generatorSettings[1].offset.x = _generatorSettings[2].width * index * 1f / _generatorSettings[1].perlinScale;
        Texture2D texture = TextureGenerator.GenerateTexture(_generatorSettings[2]);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.8f, .5f));
    }
}
