using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MinimapIconSprites))]
public class MinimapIconEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MinimapIconSprites data = (MinimapIconSprites)target;

        // Bake Button
        if (GUILayout.Button("Extract Heads"))
        {
            ExtractHeads(data);
        }
    }

    private void ExtractHeads(MinimapIconSprites data)
    {
        EditorUtility.SetDirty(data);
        Debug.Log("Extracted Employee Heads!");

        AssetDatabase.SaveAssets();
    }

    public static Sprite ExtractHead(MinimapIconBaseSprite source)
    {
        Texture2D tex = source.sprite.texture;

        int headWidth = 6;
        int longHeadHeight = 5;
        int shortHeadHeight = 4;
        int headHeight = source.longHair ? longHeadHeight : shortHeadHeight;
        int startX = 0;
        int startY = (int)(tex.height - headHeight);

        Color[] pixels = tex.GetPixels(startX, startY, headWidth, headHeight);

        if (source.longHair)
        {
            for (int x = 1; x <= 4; x++)
            {
                pixels[x] = Color.clear; // bottom row is index 0–5 in row-major order
            }
        }

        Texture2D headTex = new(headWidth, longHeadHeight, TextureFormat.ARGB32, false);
        headTex.SetPixels(pixels);
        headTex.Apply();

        // Create sprite
        Sprite headSprite = Sprite.Create(
            headTex,
            new Rect(0, 0, headWidth, headHeight),
            new Vector2(0.5f, 2/5f),
            source.sprite.pixelsPerUnit
        );

        return headSprite;
    }
}