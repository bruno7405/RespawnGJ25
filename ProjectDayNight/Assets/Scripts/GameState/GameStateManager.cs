using System;
using UnityEngine;

public class GameStateManager : StateMachineManager
{
    private static GameStateManager instance;
    public static GameStateManager Instance => instance;

    [SerializeField] float secondsPerGameDay;

    // Day and Night
    [SerializeField] DayState dayState;
    [SerializeField] NightState nightState;
    TimeUI timeUI;

    // Events
    [SerializeField] GameObject testEvent;

    public event Action NewDay;
    public event Action NightStart;

    public void InvokeNewDay()
    {
        NewDay?.Invoke();
    }

    public void InvokeNightStart()
    {
        NightStart?.Invoke();
    }

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
