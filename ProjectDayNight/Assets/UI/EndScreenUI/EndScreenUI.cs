using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{

    [SerializeField] private string sceneToLoad = "MainGame";

    [Header("UI References")]
    [SerializeField] GameObject root;
    [SerializeField] TextMeshProUGUI highScoreTMP;
    [SerializeField] TextMeshProUGUI reasonTMP;
    [SerializeField] Image backgroundImage;


    [Header("Source Images")]
    [SerializeField] Sprite gameWinSprite;
    [SerializeField] Sprite gameLoseSprite;

    private void Awake()
    {
        root.SetActive(false);
    }

    public void Display(bool didWin, int score, string reason)
    {
        root.SetActive(true);
        if (didWin)
        {
            backgroundImage.sprite = gameWinSprite;
            highScoreTMP.text = "You Won!";
            highScoreTMP.color = Color.forestGreen;
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
