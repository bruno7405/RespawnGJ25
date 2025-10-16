using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    private static PlayerStatsUI instance;
    public static PlayerStatsUI Instance => instance;

    [SerializeField] TextMeshProUGUI moneyTMP;


    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this);
        }
        instance = this;
    }

    public void SetMoney(int money)
    {
        moneyTMP.text = "$ " + money.ToString();
    }
}
