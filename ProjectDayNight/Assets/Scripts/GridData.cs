using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GridData", menuName = "Pathfinding/GridData")]
public class GridData : ScriptableObject
{
    [Header("Baked Data")]
    public int Width;
    public int Height;
    private bool[] walkableGrid;

    public bool IsWalkable(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return false;
        return walkableGrid[y * Width + x];
    }

    public void SetWalkableGrid(bool[] newGrid)
    {
        if (newGrid.Length != Width * Height)
            throw new ArgumentException("Grid size does not match. Set dimensions first");
        walkableGrid = newGrid;
    }
}
