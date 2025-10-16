using UnityEngine;

[System.Serializable]
public struct MinimapIconSource
{
    public bool longHair;
    public Sprite sprite;
    public string name;
}
[System.Serializable]
public struct MinimapIcon
{
    public Sprite sprite;
    public string name;
}

[CreateAssetMenu(fileName = "MinimapIconSprites", menuName = "Sprites/MinimapIconSprites")]
public class MinimapIconSprites : ScriptableObject
{
    [SerializeField] private MinimapIconSource boss;
    public MinimapIconSource Boss => boss;
    [SerializeField] private Sprite bossHead;
    public MinimapIcon BossHead => new () { name = boss.name, sprite = bossHead };
    [SerializeField] private Sprite bossTask;
    public MinimapIcon BossTask => new () { name = boss.name, sprite = bossTask };
    [SerializeField] private MinimapIconSource[] employees;
    public MinimapIconSource[] Employees => employees;
    [SerializeField] private MinimapIcon[] employeeHeads;
    public MinimapIcon[] EmployeeHeads => employeeHeads;

    public void SetEmployeeHeads(MinimapIcon[] heads)
    {
        employeeHeads = heads;
    }

    public MinimapIcon GetMinimapIcon(string name)
    {
        foreach (var icon in employeeHeads)
        {
            if (icon.name == name)
                return icon;
        }
        Debug.LogWarning($"No head sprite found for name: {name}");
        return default;
    }
}