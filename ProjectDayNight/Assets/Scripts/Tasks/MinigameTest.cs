using UnityEngine;

public class MinigameTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MinigameManager.Instance.StartAimTrainer(Win, Lose);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Win()
    {
        InformationPopupUI.Instance.DisplayText("You won the minigame!", true, 2f);
    }
    void Lose()
    {
        InformationPopupUI.Instance.DisplayText("You lost the minigame!", false, 2f);
    }
}
