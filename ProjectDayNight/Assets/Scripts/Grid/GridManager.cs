using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    public static Tilemap GroundTilemap;

    // For future proofing (multiple scenes), should make this a singleton
    private void Awake() => GroundTilemap = groundTilemap;

    public static void NullCheck()
    {
        if (GroundTilemap == null) throw new System.Exception("Ground Tilemap is not set.");
    }

    public static Vector2Int PositionToCell(Vector2 position)
    {
        NullCheck();
        return (Vector2Int)GroundTilemap.WorldToCell(position);
    }

    public static Vector2 WorldTileCenter(Vector2Int coord)
    {
        NullCheck();
        return GroundTilemap.CellToWorld((Vector3Int)coord) + GroundTilemap.cellSize / 2;
    }

    // Lazy implementation, relies on unit size tilemap tiles
    public static Vector2 WorldTileCenter(Vector2 worldCoord)
    {
        NullCheck();
        return Vector2Int.RoundToInt(worldCoord) + (Vector2)GroundTilemap.cellSize / 2;
    }

    public static Vector2 RandomWalkablePos()
    {
        int x, y;
        do
        {
            x = Random.Range(0, GridData.Instance.Width);
            y = Random.Range(0, GridData.Instance.Height);
        } while (!GridData.Instance.IsWalkable(x, y));
        return WorldTileCenter(new(x, y));
    }
}
