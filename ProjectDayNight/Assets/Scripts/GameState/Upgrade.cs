using UnityEngine;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public enum Category
    {
        productivity,
        morale
    }

    public string upgradeName;
    public Sprite image;
    public string description;
    public Category stat;
    public int increasePercentage;

}
