using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static GridManager;
using System.Linq;

public class MinimapManager : MonoBehaviour
{
    private static MinimapManager instance;
    public static MinimapManager Instance => instance;
    private static MinimapIconSprites iconData;

    [SerializeField] GameObject root;
    [SerializeField] private RawImage minimapImage;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private int tilePixelSize;
    [SerializeField] private Color floorColor;
    [SerializeField] private Color wallColor;
    private int borderWidth => tilePixelSize / 2;
    private Dictionary<Employee, RectTransform> employeeIcons;
    private RectTransform bossIcon;
    public Vector2Int MinimapSize { get; private set; }
    private bool[,] grid; // your boolean grid
    public Vector2 PlayerPos => PlayerMovement.Instance.transform.position;

    void Awake()
    {
        instance = this;
        iconData = Resources.Load<MinimapIconSprites>("MinimapIconSprites");
        employeeIcons = new();
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
        GameStateManager.Instance.NightStart += HandleNightStart;
    }

    void Update()
    {
        if (bossIcon != null)
        {
            bossIcon.anchoredPosition = WorldToMinimap(PlayerPos);
        }
        if (employeeIcons.Count > 0)
        {
            employeeIcons.ToList().ForEach(kvp => kvp.Value.anchoredPosition = WorldToMinimap(kvp.Key.transform.position));
        }
    }

    private void HandleNightStart()
    {
        foreach (Transform child in minimapImage.transform)
        {
            string childName = child.gameObject.name; 
            if (childName == iconData.BossHead.name || employeeIcons.Any(kvp => kvp.Key.Name == childName)) continue;
            Destroy(child.gameObject);
        }
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

                tex.SetPixels(x * tilePixelSize + borderWidth, y * tilePixelSize + borderWidth, tilePixelSize, tilePixelSize, isFloor ? floorSquare : wallSquare);
            }
        }

        // Debug.Log($"Grid Size: {GridWidth}x{GridHeight} Minimap generated with size: {MinimapSize.x}x{MinimapSize.y} Texture size: {tex.width}x{tex.height} TilePixelSize: {tilePixelSize}");
        tex.Apply();
        minimapImage.rectTransform.sizeDelta = MinimapSize;
        minimapImage.texture = tex;
    }

    public void RegisterEmployee(Employee employee)
    {
        // Debug.Log($"Registering employee {employee.Name} on minimap");
        string name = employee.Name;
        MinimapIcon headIcon = iconData.GetMinimapIcon(name);
        RectTransform rt = AddIcon(headIcon, employee.transform.position);
        employeeIcons.Add(employee, rt);
    }
    public void UnregisterEmployee(Employee employee)
    {
        employeeIcons.Remove(employee);
        foreach (Transform child in minimapImage.transform)
        {
            if (child.name == employee.Name)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    public void RegisterTask(TaskBruno task)
    {
        // Debug.Log($"Registering task {task.Name} on minimap");
        MinimapIcon taskIcon = iconData.BossTask;
        AddIcon(taskIcon, task.transform.position, new(1/2f, 4/3f), task.Name);
    }
    public void UnregisterTask(TaskBruno task)
    {
        foreach (Transform child in minimapImage.transform)
        {
            if (child.name == task.Name)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    public void RegisterBoss()
    {
        bossIcon = AddIcon(iconData.BossHead, PlayerPos);
    }
    public void UnregisterBoss()
    {
        foreach (Transform child in minimapImage.transform)
        {
            if (child.name == iconData.BossHead.name)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    public RectTransform AddIcon(MinimapIcon icon, Vector2 worldPos, Vector2 sizeScale = default, string name = null)
    {
        if (sizeScale == default) sizeScale = Vector2.one;
        
        Vector2 anchoredPos = WorldToMinimap(worldPos);
        GameObject instane = Instantiate(iconPrefab, minimapImage.transform);
        RectTransform rt = instane.GetComponent<RectTransform>();

        instane.name = name ?? icon.name;
        instane.GetComponent<Image>().sprite = icon.sprite;
        rt.sizeDelta = new(2 * tilePixelSize * sizeScale.x, 2 * tilePixelSize * sizeScale.y);
        rt.anchoredPosition = anchoredPos;
        
        return rt;
    }

    private Vector2 WorldToMinimap(Vector2 worldPos)
    {
        float normX = (worldPos.x - WorldMin.x) / WorldSize.x - 0.5f;
        float normY = (worldPos.y - WorldMin.y) / WorldSize.y - 0.5f;
        RectTransform rt = minimapImage.rectTransform;
        return new(normX * rt.rect.width, normY * rt.rect.height);
    }

    public void Toggle()
    {
        if (!root.activeInHierarchy)
        {
            root.SetActive(true);
            // PlayerMovement.Instance.DisableMovement();
        }
        else
        {
            // PlayerMovement.Instance.EnableMovement();
            root.SetActive(false);
        }

            AudioManager.Instance.PlayOneShot("FileCabinetPages", 0.05f);
    }
}
