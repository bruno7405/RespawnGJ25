using UnityEngine;

public struct StatusIconSprite
{
    public string name;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "StatusIconSprites", menuName = "Sprites/StatusIconSprites")]
public class StatusIconSprites : ScriptableObject
{
    [SerializeField] private Sprite happySprite;
    public Sprite HappySprite => happySprite;
    [SerializeField] private Sprite neutralSprite;
    public Sprite NeutralSprite => neutralSprite;
    [SerializeField] private Sprite upsetSprite;
    public Sprite UpsetSprite => upsetSprite;
    [SerializeField] private Sprite slackingOffSprite;
    public Sprite SlackingOffSprite => slackingOffSprite;
}