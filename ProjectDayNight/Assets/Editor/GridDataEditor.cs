using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(GridData))]
public class GridDataEditor : Editor
{
    private Grid tilemapGrid;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        tilemapGrid = (Grid)EditorGUILayout.ObjectField("Tilemap Grid", tilemapGrid, typeof(Grid), true);

        GUI.enabled = tilemapGrid != null;
        if (GUILayout.Button("Generate From Tilemap Layers"))
        {
            GenerateFromTilemaps((GridData)target);
        }
        GUI.enabled = true;
        if (GUILayout.Button("Display Walkable Data"))
        {
            DisplayGridData((GridData)target);
        }
    }

    private void GenerateFromTilemaps(GridData data)
    {
        if (tilemapGrid == null)
        {
            Debug.LogWarning("No assigned grid source");
            return;
        }

        Tilemap[] tilemaps = tilemapGrid.GetComponentsInChildren<Tilemap>();
        if (tilemaps.Length == 0)
        {
            Debug.LogWarning("No Tilemaps found under this Grid.");
            return;
        }

        // Assume tilemaps[0] is walkable layer, others not
        Tilemap walkableLayer = tilemaps[0];

        List<Vector3Int> positions = new();
        foreach (var pos in walkableLayer.cellBounds.allPositionsWithin)
        {
            if (walkableLayer.HasTile(pos))
                positions.Add(pos);
        }
        int minX = positions.Min(p => p.x);
        int maxX = positions.Max(p => p.x);
        int minY = positions.Min(p => p.y);
        int maxY = positions.Max(p => p.y);

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;

        bool[] walkableGrid = new bool[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3Int cellPos = new(minX + x, minY + y, 0);
                if (!walkableLayer.HasTile(cellPos)) continue;
                bool isWalkable = true;

                // Check all higher layers for blocking tiles
                for (int layer = 1; layer < tilemaps.Length; layer++)
                {
                    if (tilemaps[layer].HasTile(cellPos))
                    {
                        isWalkable = false;
                        break;
                    }
                }

                walkableGrid[y * width + x] = isWalkable;
            }
        }

        data.SetGridData(walkableGrid, new(width, height), new(minX, minY));

        EditorUtility.SetDirty(data);
        Debug.Log("GridData generated from tilemap layers!");

        AssetDatabase.SaveAssets();
    }

    private void DisplayGridData(GridData data)
    {
        string gridString = "";
        for (int y = data.Height - 1; y >= 0; y--)
        {
            gridString += "y=" + y + ":\t\t\t";
            for (int x = 0; x < data.Width; x++)
            {
                gridString += data.IsWalkable(x, y) ? "1\t" : "0\t";
            }
            gridString += "\n";
        }
        Debug.Log(gridString);
    }
}
