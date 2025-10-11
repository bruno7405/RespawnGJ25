using UnityEngine;

public class GameStateManager : StateMachineManager
{
    public static GameStateManager instance;

    [SerializeField] float secondsPerGameDay;

    // Day and Night
    [SerializeField] DayState dayState;
    [SerializeField] NightState nightState;
    TimeUI timeUI;

    // Events
    [SerializeField] GameObject testEvent;



    private void Awake()
    {
        instance = this;

        timeUI = GetComponentInChildren<TimeUI>();
        timeUI.SetSecondsPerGameDay(secondsPerGameDay);

        dayState.SetDuration(secondsPerGameDay / 2);
        nightState.SetDuration(secondsPerGameDay / 2);
        
    }


    public void AddUpgrade()
    {

    }

    public void GameOver()
    {
        Debug.Log("game over!");
    }


}
