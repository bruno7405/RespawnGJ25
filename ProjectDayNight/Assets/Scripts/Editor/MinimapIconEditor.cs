using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(MinimapIconSprites))]
public class MinimapIconEditor : Editor
{
    bool overwriteDisk = false;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MinimapIconSprites data = (MinimapIconSprites)target;

        overwriteDisk = EditorGUILayout.Toggle("Overwrite Disk", overwriteDisk);

        // Bake Button
        if (GUILayout.Button("Extract Employee Heads"))
        {
            ExtractEmployeeHeads(data);
        }

        if (GUILayout.Button("Extract Boss Head"))
        {
            ExtractBossHead(data);
        }
    }

    private void ExtractEmployeeHeads(MinimapIconSprites data)
    {
        data.SetEmployeeHeads(Array.ConvertAll(data.Employees, ExtractHead));
        EditorUtility.SetDirty(data);
        Debug.Log("Extracted Employee Heads!");

        AssetDatabase.SaveAssets();
    }
    private void ExtractBossHead(MinimapIconSprites data)
    {
        data.SetBossHead(ExtractHead(data.Boss));
        EditorUtility.SetDirty(data);
        Debug.Log("Extracted Boss Head!");

        AssetDatabase.SaveAssets();
    }

    public MinimapIcon ExtractHead(MinimapIconSource source)
    {
        Texture2D tex = source.sprite.texture;

        int headWidth = 6;
        int longHeadHeight = 5;
        int shortHeadHeight = 4;
        int headHeight = source.longHair ? longHeadHeight : shortHeadHeight;
        int startX = 0;
        int startY = tex.height - headHeight;

        Color[] pixels = tex.GetPixels(startX, startY, headWidth, headHeight);

        if (source.longHair)
        {
            for (int i = 1; i <= 4; i++)
            {
                pixels[i] = Color.clear; // bottom row is index 0–5 in row-major order
            }
        } else
        {
            Color[] newPixels = new Color[headWidth * longHeadHeight];
            for (int i = 0; i <= 5; i++)
            {
                newPixels[i] = Color.clear;
            }
            Array.Copy(pixels, 0, newPixels, 6, pixels.Length);
            pixels = newPixels;
        }

        Texture2D headTex = new(headWidth, longHeadHeight, TextureFormat.ARGB32, false);
        headTex.SetPixels(pixels);
        headTex.Apply();

        if (overwriteDisk)
        {
            string path = "Assets/Sprite/MinimapHeads/" + source.name + "Head.png";
            byte[] pngData = headTex.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, pngData);
            AssetDatabase.ImportAsset(path);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.filterMode = FilterMode.Point;
            importer.textureType = TextureImporterType.Sprite;
            importer.mipmapEnabled = false;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.spritePivot = new Vector2(0.5f, source.longHair ? 0.5f : 0.6f);
            importer.spritePixelsPerUnit = 1;
            importer.SaveAndReimport();
        }
        Sprite importedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprite/MinimapHeads/" + source.name + "Head.png");
        return new MinimapIcon { sprite = importedSprite, name = source.name };
    }
}