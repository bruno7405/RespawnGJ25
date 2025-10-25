using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : StateMachineManager
{
    private static GameStateManager instance;
    public static GameStateManager Instance => instance;

    [Header("Time Params")]
    [SerializeField] float secondsPerGameDay;
    [Range(0f, 10f)][SerializeField] float timeScale = 1f;

    [Header("UI")]
    [SerializeField] BlackScreenUI blackScreenUI;
    [SerializeField] DayNightTransitionUI dayNightTransitionUI;
    [SerializeField] EndScreenUI endScreenUI;


    // Day and Night
    [SerializeField] DayState dayState;
    public DayState DayState => dayState;
    [SerializeField] NightState nightState;
    public NightState NightState => nightState;
    TimeUI timeUI;
    [SerializeField] int currentDay = 0;
    public int CurrentDay => currentDay;
    public event Action DayStart;
    public event Action NightStart;
    public bool IsDay => currentState == dayState;
    public int Mistakes { get; private set; } = 0;

    public void IncrementMistakes()
    {
        Mistakes++;
        PlayerStatsUI.Instance.SetLives(3 - Mistakes);
        AudioManager.Instance.PlayOneShot("Error");
        if (Mistakes >= 3)
        {
            StartCoroutine(GameOver());
        }
    }

    public void InvokeNewDay()
    {
        if (currentState != dayState) return;
        currentDay++;
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
        
        // Time UI
        timeUI = GetComponentInChildren<TimeUI>();
        timeUI.SetSecondsPerGameDay(secondsPerGameDay);
        dayState.SetDuration(secondsPerGameDay / 2);
        nightState.SetDuration(secondsPerGameDay / 2);
    }
    public IEnumerator GameOver()
    {
        PlayerInput.active = false;
        blackScreenUI.FadeIn();
        yield return new WaitForSeconds(1);
        blackScreenUI.FadeOut();
        endScreenUI.Display(false, CompanyManager.Instance.Money, "You've been busted");
        yield return new WaitForSeconds(3);
        blackScreenUI.FadeIn();
        yield return new WaitForSeconds(0.5f);
        PlayerInput.active = true;
        SceneManager.LoadScene(0); // Main Menu Scene
    }

}
