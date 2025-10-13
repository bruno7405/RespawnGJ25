using System;
using UnityEngine;

public class GameStateManager : StateMachineManager
{
    private static GameStateManager instance;
    public static GameStateManager Instance => instance;

    [SerializeField] float secondsPerGameDay;

    // Day and Night
    [SerializeField] DayState dayState;
    public DayState DayState => dayState;
    [SerializeField] NightState nightState;
    public NightState NightState => nightState;
    TimeUI timeUI;

    public int CurrentDay { get; private set; } = 0;

    public event Action NewDay;
    public event Action NightStart;

    public void InvokeNewDay()
    {
        CurrentDay++;
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
