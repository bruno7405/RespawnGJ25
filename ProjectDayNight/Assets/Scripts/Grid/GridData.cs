using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GridData", menuName = "Pathfinding/GridData")]
public class GridData : ScriptableObject
{
    private int width;
    public int Width => width;
    private int height;
    public int Height => height;
    private bool[] walkableGrid;

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

    public bool IsWalkable(int x, int y)
    {
        if (walkableGrid == null || walkableGrid.Length == 0) throw new InvalidOperationException("Walkable grid is empty or not initialized");
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return false;
        return walkableGrid[y * Width + x];
    }

    public void SetWalkableGrid(bool[] newGrid, Vector2Int size)
    {
        if (newGrid.Length != size.x * size.y)
            throw new ArgumentException("Grid size does not match. Set dimensions first");
        width = size.x;
        height = size.y;
        walkableGrid = newGrid;
    }
}
