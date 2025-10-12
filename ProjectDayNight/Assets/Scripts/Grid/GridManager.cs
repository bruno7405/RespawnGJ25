using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static Tilemap GroundTilemap;
    public static GameObject tempStart;
    public static GameObject tempGoal;

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private GameObject tempStartPrefab;
    [SerializeField] private GameObject tempGoalPrefab;

    private void Awake()
    {
        GroundTilemap = groundTilemap;
        tempStart = tempStartPrefab;
        tempGoal = tempGoalPrefab;
    }

    public static Vector2Int PositionToCell(Vector2 position)
    {
        if (GroundTilemap == null) throw new System.Exception("Ground Tilemap is not set.");
        return (Vector2Int)GroundTilemap.WorldToCell(position);
    }

    public static Vector2 WorldTileCenter(Vector2Int coord)
    {
        return GroundTilemap.CellToWorld((Vector3Int)coord) + GroundTilemap.cellSize / 2;
    }

    // Lazy implementation, relies on unit size tilemap tiles
    public static Vector2 WorldTileCenter(Vector2 worldCoord)
    {
        return Vector2Int.RoundToInt(worldCoord) + (Vector2)GroundTilemap.cellSize / 2;
    }
}
