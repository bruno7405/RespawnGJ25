using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : StateMachineManager
{
    private static GameStateManager instance;
    public static GameStateManager Instance => instance;

    [SerializeField] float secondsPerGameDay;

    [Range(0f, 10f)]
    [SerializeField] float timeScale = 1f;


    // Day and Night
    [SerializeField] DayState dayState;
    public DayState DayState => dayState;
    [SerializeField] NightState nightState;
    public NightState NightState => nightState;
    TimeUI timeUI;
    public int CurrentDay { get; private set; } = 0;
    public event Action DayStart;
    public event Action NightStart;
    public bool IsDay => currentState == dayState;

    public void InvokeNewDay()
    {
        CurrentDay++;
        DayStart?.Invoke();
    }

    public void InvokeNightStart()
    {
        NightStart?.Invoke();
    }

    private void Awake()
    {
        Debug.Log("GameStateManager instance set");
        instance = this;
        Time.timeScale = timeScale;

        timeUI = GetComponentInChildren<TimeUI>();
        timeUI.SetSecondsPerGameDay(secondsPerGameDay);

        dayState.SetDuration(secondsPerGameDay / 2);
        //nightState.SetDuration(secondsPerGameDay / 2);
    }

    public void AddUpgrade()
    {

    }

    public void GameOver()
    {
        Debug.Log("game over!");
    }

}
