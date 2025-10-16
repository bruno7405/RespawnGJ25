using UnityEngine;

[System.Serializable]
public struct MinimapIconBaseSprite
{
    public bool longHair;
    public Sprite sprite;
    public string name;
}

[CreateAssetMenu(fileName = "MinimapIconSprites", menuName = "Sprites/MinimapIconSprites")]
public class MinimapIconSprites : ScriptableObject
{
    [SerializeField] private MinimapIconBaseSprite boss;
    public MinimapIconBaseSprite Boss => boss;
    [SerializeField] private Sprite bossHead;
    public Sprite BossHead => bossHead;
    [SerializeField] private Sprite bossTask;
    public Sprite BossTask => bossTask;
    [SerializeField] private MinimapIconBaseSprite[] employees;
    public MinimapIconBaseSprite[] Employees => employees;
    [SerializeField] private Sprite[] employeeHeads;
    public Sprite[] EmployeeHeads => employeeHeads;
}