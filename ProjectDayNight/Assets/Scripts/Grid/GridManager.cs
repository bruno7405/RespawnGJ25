using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
public class GridManager : MonoBehaviour
{
    [SerializeField] private bool printGridOnGeneration;
    [SerializeField] private Grid grid;
    private static Vector2Int gridSize;
    public static int GridWidth => gridSize.x;
    public static int GridHeight => gridSize.y;
    private static bool[] walkableGrid;
    private static Vector2Int offset;
    public static Vector2 WorldMin;
    public static Vector2 WorldMax;
    public static Vector2 WorldSize => WorldMax - WorldMin;
    public static Grid Grid;
    public static Tilemap FloorTilemap;

    // For future proofing (multiple scenes), should make this a singleton
    private void Awake()
    {
        Grid = grid;
        FloorTilemap = Grid.GetComponentInChildren<Tilemap>(); // First tilemap must be floor
        GenerateFromTilemaps();
    }

    public static void NullCheck()
    {
        if (Grid == null || FloorTilemap == null) throw new Exception("Grid or floor tilemap is not set.");
    }

    public static bool IsWalkable(Vector2Int cell, bool isTilemapDomain = true)
    {
        if (walkableGrid == null || walkableGrid.Length == 0) throw new InvalidOperationException("Walkable grid is empty or not initialized");
        int x = cell.x;
        int y = cell.y;
        if (isTilemapDomain)
        {
            x -= offset.x;
            y -= offset.y;
        }
        if (x < 0 || x >= GridWidth || y < 0 || y >= GridHeight)
            return false;
        return walkableGrid[y * GridWidth + x];
    }
    public static bool IsInBounds(Vector2 pos, bool isTilemapDomain = true)
    {
        float x = pos.x;
        float y = pos.y;
        if (isTilemapDomain)
        {
            x -= offset.x;
            y -= offset.y;
        }
        return x >= 0 && x < GridWidth && y >= 0 && y < GridHeight;
    }

    public void SetGridData(bool[] newGrid, Vector2Int size, Vector2Int offset)
    {
        if (newGrid.Length != size.x * size.y) throw new ArgumentException("Impossible grid size given size");
        gridSize = size;
        GridManager.offset = offset;
        walkableGrid = newGrid;
    }

    private void GenerateFromTilemaps()
    {
        NullCheck();

        Tilemap[] tilemaps = Grid.GetComponentsInChildren<Tilemap>();
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
        WorldMin = walkableLayer.CellToWorld(new(minX, minY));
        WorldMax = walkableLayer.CellToWorld(new(maxX + 1, maxY + 1)); // +1 to get top-right tile corner

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

        SetGridData(walkableGrid, new(width, height), new(minX, minY));

        if (printGridOnGeneration)
        {
            string gridString = "";
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    gridString += (walkableGrid[y * width + x] ? "1" : "0") + "\t";
                }
                gridString += "\n";
            }
            Debug.Log(gridString);
        }

        Debug.Log("GridData generated from tilemap layers!");
    }

    public static Vector2Int getFloorOffset(Tilemap walkableTilemap)
    {
        List<Vector3Int> positions = new();
        foreach (var pos in walkableTilemap.cellBounds.allPositionsWithin)
        {
            if (walkableTilemap.HasTile(pos))
                positions.Add(pos);
        }
        return new(positions.Min(p => p.x), positions.Min(p => p.y));
    }

    public static Vector2Int PositionToCell(Vector2 position)
    {
        NullCheck();
        return (Vector2Int)FloorTilemap.WorldToCell(position);
    }

    public static Vector2 WorldTileCenter(Vector2Int coord)
    {
        NullCheck();
        return FloorTilemap.CellToWorld((Vector3Int)coord) + FloorTilemap.cellSize / 2;
    }

    public static Vector2 RandomWalkablePos()
    {
        int x, y;
        do
        {
            x = Random.Range(0, GridWidth);
            y = Random.Range(0, GridHeight);
        } while (!IsWalkable(new(x, y)));
        return WorldTileCenter(new(x, y));
    }
}
