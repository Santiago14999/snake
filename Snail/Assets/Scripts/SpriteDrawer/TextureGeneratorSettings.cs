using UnityEngine;

[CreateAssetMenu(fileName = "NewSpriteGeneratorSettings", menuName = "ScriptableObjects/Sprite Generator Settings")]
public class TextureGeneratorSettings : ScriptableObject
{
    public int width;
    public int height;
    public int seed;
    public Vector2 offset;
    public Vector2 grayscale;
    public float perlinScale;
    public TextureGenerator.ColorLevel[] colors;
    public bool pointFilter;
    [Header("Outline")]
    public bool outline;
    public bool leftOutline;
    public bool rightOutline;
    public TextureGenerator.ColorLevel[] outlineColors;
    [Header("Border Radius")]
    public bool leftRadius;
    public bool rightRadius;
    public int borderRadius;
    public Texture2D overlayTexture;

    public TextureGeneratorSettings Copy()
    {
        TextureGeneratorSettings copy = CreateInstance<TextureGeneratorSettings>();
        copy.width = width;
        copy.height = height;
        copy.seed = seed;
        copy.offset = offset;
        copy.grayscale = grayscale;
        copy.perlinScale = perlinScale;
        copy.colors = colors;
        copy.pointFilter = pointFilter;
        copy.outline = outline;
        copy.leftOutline = leftOutline;
        copy.rightOutline = rightOutline;
        copy.outlineColors = outlineColors;
        copy.leftRadius = leftRadius;
        copy.rightRadius = rightRadius;
        copy.borderRadius = borderRadius;
        copy.overlayTexture = overlayTexture;

        return copy;
    }
}
