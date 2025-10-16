using UnityEngine;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public enum Category
    {
        productivity,
        morale,
        playerSpeed,
        playerInteractRange,
        playerLightRange
    }

    public string upgradeName;
    public Sprite image;
    public Color backgroundColor;
    public string description;
    public Category category;
    public int increasePercentage;

}
