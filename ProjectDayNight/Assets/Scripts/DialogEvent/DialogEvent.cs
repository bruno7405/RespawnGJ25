using UnityEngine;

[System.Serializable]
public class DialogChoice
{
    public string choiceText;
    public bool isCorrect; // mark which option is correct
}

[System.Serializable]
public class DialogLine
{
    [TextArea]
    public string npcLine;
    public DialogChoice[] choices = new DialogChoice[2]; // 2 choices
}

[CreateAssetMenu(fileName = "DialogEvent", menuName = "Dialogue/DialogEvent")]
public class DialogEvent : ScriptableObject
{
    public string npcName;
    public DialogLine[] lines = new DialogLine[2];
}
