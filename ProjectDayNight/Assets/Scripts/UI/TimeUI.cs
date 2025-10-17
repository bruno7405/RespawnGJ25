using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] TextMeshProUGUI timeText;
    float secondsPerGameDay;
    float timeElapsed;
    float timePercentage;
    float hour;

    void Start()
    {
        
    }

    void Update()
    {
        UpdateTimeText();
    }

    #region Time UI
    void UpdateTimeText()
    {
        // display time to text UI
        timePercentage = timeElapsed / secondsPerGameDay;
        hour = Mathf.Lerp(0, 24, Mathf.Clamp(timePercentage, 0, 1));
        int minutes = (int) Mathf.Lerp(0, 60, hour % 1);
        timeToText((int) hour + 9, minutes);
    }

    void timeToText(int hour, int min)
    {
        if (hour < 10)
        {
            if (min < 10)
            {
                timeText.text = "0" + hour + ":" + "0" + min;
            }
            else
            {
                timeText.text = "0" + hour + ":" + min;
            }
        }
        else if (hour < 24)
        {
            if (min < 10)
            {
                timeText.text = hour + ":" + "0" + min;
            }
            else
            {
                timeText.text = hour + ":" + min;
            }
        }
        else
        {
            if (min < 10)
            {
                timeText.text = "0" + (hour - 24) + ":" + "0" + min;
            }
            else
            {
                timeText.text = "0" + (hour  - 24) + ":" + min;
            }
        }
    }
    #endregion

    public void SetSecondsPerGameDay(float s)
    {
        secondsPerGameDay = s;
    }

    public void SetTimeElasped(float t, bool isDay)
    {
        if (isDay) timeElapsed = t;
        else timeElapsed = t + secondsPerGameDay/2;
    }

    public void EnableUI()
    {
        root.SetActive(true);
    }

    public void DisableUI()
    {
        root.SetActive(false);
    }
}
