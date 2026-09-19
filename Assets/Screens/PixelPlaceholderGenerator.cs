#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class PixelPlaceholderGenerator
{
    [MenuItem("Tools/Generate Pixel Art Placeholders")]
    public static void Generate()
    {
        string dir = "Assets/Placeholders";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        CreateIcon(dir + "/Gold.png", new Color(0.9f, 0.75f, 0.2f), "coin");
        CreateIcon(dir + "/Wood.png", new Color(0.55f, 0.35f, 0.15f), "box");
        CreateIcon(dir + "/Stone.png", new Color(0.5f, 0.5f, 0.55f), "rock");
        CreateIcon(dir + "/PanelBorder.png", new Color(0.18f, 0.14f, 0.1f), "frame");

        AssetDatabase.Refresh();
    }

    private static void CreateIcon(string path, Color baseColor, string shape)
    {
        Texture2D tex = new Texture2D(16, 16, TextureFormat.RGBA32, false);
        Color empty = new Color(0, 0, 0, 0);
        Color border = baseColor * 0.35f;
        border.a = 1f;

        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 16; x++)
            {
                bool isBorder = (x == 1 || x == 14 || y == 1 || y == 14);
                bool isFill = (x > 1 && x < 14 && y > 1 && y < 14);

                if (shape == "frame")
                    tex.SetPixel(x, y, (x == 0 || x == 15 || y == 0 || y == 15) ? border : (isBorder ? baseColor : empty));
                else if (isBorder)
                    tex.SetPixel(x, y, border);
                else if (isFill)
                    tex.SetPixel(x, y, baseColor);
                else
                    tex.SetPixel(x, y, empty);
            }
        }

        tex.Apply();
        File.WriteAllBytes(path, tex.EncodeToPNG());

        AssetDatabase.ImportAsset(path);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }
    }
}
#endif