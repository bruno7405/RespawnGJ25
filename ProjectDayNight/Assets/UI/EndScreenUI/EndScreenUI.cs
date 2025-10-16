using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndBackground : MonoBehaviour
{

    [SerializeField] private string sceneToLoad = "MainGame";

    [Header("UI References")]
    [SerializeField] BlackScreenUI blackScreenUI;
    [SerializeField] TextMeshProUGUI highScoreTMP;
    [SerializeField] TextMeshProUGUI reasonTMP;
    [SerializeField] Image backgroundImage;


    [Header("Source Images")]
    [SerializeField] Sprite gameWinSprite;
    [SerializeField] Sprite gameLoseSprite;

    public void ApplyEndScreenState(bool didWin, int score, string reason)
    {
        if (didWin)
        {
            backgroundImage.sprite = gameWinSprite;
            highScoreTMP.text = "You Won!";
        }
        else
        {
            backgroundImage.sprite = gameLoseSprite;
            highScoreTMP.text = "Game Over";
        }

        reasonTMP.text = reason;
        highScoreTMP.text = score.ToString();
    }
}
