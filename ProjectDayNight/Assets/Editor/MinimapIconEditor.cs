using UnityEngine;
using UnityEditor;
using System;

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
        data.SetEmployeeHeads(Array.ConvertAll(data.Employees, ExtractHead));
        EditorUtility.SetDirty(data);
        Debug.Log("Extracted Employee Heads!");

        AssetDatabase.SaveAssets();
    }

    public static MinimapIcon ExtractHead(MinimapIconSource source)
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

        Texture2D headTex = new(headWidth, longHeadHeight, TextureFormat.ARGB32, false) {
            filterMode = FilterMode.Point
        };
        headTex.SetPixels(pixels);
        headTex.Apply();
        
        string path = "Assets/Sprite/MinimapHeads/" + source.name + "Head.png";
        byte[] pngData = headTex.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, pngData);
        AssetDatabase.ImportAsset(path);
        Texture2D importedTex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        Sprite assetSprite = Sprite.Create(importedTex, new(0, 0, importedTex.width, importedTex.height), new(0.5f, 2 / 5f));
        return new MinimapIcon { sprite = assetSprite, name = source.name };
    }
}