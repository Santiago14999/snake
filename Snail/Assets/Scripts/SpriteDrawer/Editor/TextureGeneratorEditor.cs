using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(TextureGenerator))]
public class TextureGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TextureGenerator drawer = (TextureGenerator)target;
        if (DrawDefaultInspector())
        {
            if (drawer.AutoUpdate)
            {
                drawer.ApplyTexture();
            }
        }

        if (GUILayout.Button("Generate Texture"))
            drawer.ApplyTexture();

        if (GUILayout.Button("Save Texture"))
        {
            byte[] data = drawer.CurrentTexture.EncodeToPNG();
            string path = Application.dataPath + "/Textures/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += Random.Range(0, int.MaxValue - 1) + ".png";
            File.WriteAllBytes(path, data);
        }

        if (GUILayout.Button("Save Generator Settings"))
        {
            TextureGeneratorSettings settings = drawer.GetSettings();
            string path = "Assets/New Sprite Generator Settings.asset";
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(settings, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = settings;
        }

        if (GUILayout.Button("Generate Preset"))
        {
            drawer.GeneratePreset();
        }
    }
}
