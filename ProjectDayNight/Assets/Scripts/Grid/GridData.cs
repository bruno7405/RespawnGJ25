using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "GridData", menuName = "Pathfinding/GridData")]
public class GridData : ScriptableObject
{
    [SerializeField] private Vector2Int size;
    public int Width => size.x;
    public int Height => size.y;

    [SerializeField] private bool[] walkableGrid;
    [SerializeField] private Vector2Int offset;

    private static GridData _instance;
    public static GridData Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<GridData>("GridData");
            return _instance;
        }
    }

    public bool IsWalkable(int x, int y, bool isTilemapDomain = true)
    {
        if (isTilemapDomain)
        {
            x -= offset.x;
            y -= offset.y;
        }
        if (walkableGrid == null || walkableGrid.Length == 0) throw new InvalidOperationException("Walkable grid is empty or not initialized");
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return false;
        return walkableGrid[y * Width + x];
    }

    public void SetGridData(bool[] newGrid, Vector2Int size, Vector2Int offset)
    {
        if (newGrid.Length != size.x * size.y) throw new ArgumentException("Impossible grid size given size");
        this.size = size;
        this.offset = offset;
        walkableGrid = newGrid;
    }

    public static Vector2Int getOriginOffset(Tilemap walkableTilemap)
    {
        List<Vector3Int> positions = new();
        foreach (var pos in walkableTilemap.cellBounds.allPositionsWithin)
        {
            if (walkableTilemap.HasTile(pos))
                positions.Add(pos);
        }
        return new(positions.Min(p => p.x), positions.Min(p => p.y));
    }
}
