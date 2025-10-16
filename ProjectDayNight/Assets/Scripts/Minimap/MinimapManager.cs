using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static GridManager;
using System.Linq;

public class MinimapManager : MonoBehaviour
{
    private static MinimapManager instance;
    public static MinimapManager Instance => instance;

    [SerializeField] private RawImage minimapImage;
    [SerializeField] private int tilePixelSize;
    [SerializeField] private Color floorColor;
    [SerializeField] private Color wallColor;
    private int borderWidth => tilePixelSize / 2;
    private List<GameObject> employeeIcons;
    private List<GameObject> taskIcons;
    public Vector2Int MinimapSize { get; private set; }
    private bool[,] grid; // your boolean grid

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        NullCheck();
        MinimapSize = new Vector2Int(GridWidth, GridHeight) * tilePixelSize + new Vector2Int(borderWidth * 2, borderWidth * 2);
        grid = new bool[GridWidth, GridHeight];
        for (int i = 0; i < GridWidth * GridHeight; i++)
        {
            int x = i % GridWidth;
            int y = i / GridWidth;
            grid[x, y] = IsWalkable(new(x, y), false);
        }

        GenerateMinimap();
    }

    private void GenerateMinimap()
    {
        Texture2D tex = new(MinimapSize.x, MinimapSize.y)
        {
            filterMode = FilterMode.Point
        };

        var blackRect = Enumerable.Repeat(Color.black, tex.width * tex.height).ToArray();
        var floorSquare = Enumerable.Repeat(floorColor, tilePixelSize * tilePixelSize).ToArray();
        var wallSquare = Enumerable.Repeat(wallColor, tilePixelSize * tilePixelSize).ToArray();

        tex.SetPixels(0, 0, tex.width, tex.height, blackRect); // Clear texture
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                bool isFloor = grid[x, y];

                tex.SetPixels(x*tilePixelSize + borderWidth, y*tilePixelSize + borderWidth, tilePixelSize, tilePixelSize, isFloor ? floorSquare : wallSquare);
            }
        }

        Debug.Log($"Grid Size: {GridWidth}x{GridHeight} Minimap generated with size: {MinimapSize.x}x{MinimapSize.y} Texture size: {tex.width}x{tex.height} TilePixelSize: {tilePixelSize}");
        tex.Apply();
        minimapImage.rectTransform.sizeDelta = MinimapSize;
        minimapImage.texture = tex;
    }

    public void AddIcon(GameObject iconPrefab, Vector2 worldPos)
    {
        Vector2 anchoredPos = WorldToMinimap(worldPos);
        GameObject icon = Instantiate(iconPrefab, minimapImage.transform);
        icon.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
    }

    private Vector2 WorldToMinimap(Vector3 worldPos)
    {
        float normX = (worldPos.x - WorldMin.x) / WorldSize.x;
        float normY = (worldPos.z - WorldMin.y) / WorldSize.y;
        RectTransform rt = minimapImage.rectTransform;
        return new Vector2(normX * rt.rect.width, normY * rt.rect.height);
    }
}
