using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    private static PlayerStatsUI instance;
    public static PlayerStatsUI Instance => instance;

    [SerializeField] List<GameObject> blobs = new List<GameObject>();
    [SerializeField] TextMeshProUGUI moneyTMP;


    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this);
        }
        instance = this;

        SetLives(2);
    }

    public void SetMoney(int money)
    {
        moneyTMP.text = "$ " + money.ToString();
    }
    
    public void SetLives(int lives)
    {
        if (lives > 3) Debug.LogError("Can't set lives more than 3");
        for (int i = 0; i < blobs.Count; i++)
        {
            if (i < lives)
            {
                blobs[i].GetComponent<Image>().enabled = true;
            }
            else
            {
                blobs[i].GetComponent<Image>().enabled = false;
            }
        }
    }
}
