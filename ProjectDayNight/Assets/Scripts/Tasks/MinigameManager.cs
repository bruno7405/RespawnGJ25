using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class MinigameManager : MonoBehaviour
{
    private static MinigameManager instance;
    public static MinigameManager Instance => instance;
    [Header("References")]
    [SerializeField] GameObject panel;          // Assign AimTrainerPanel


    [Header("Settings")]
    [SerializeField] float roundTime = 0.5f;    // Time allowed per target
    [SerializeField] int roundsToWin = 10;       // Number of hits required

    GameObject currentTarget;
    int currentRound = 0;
    bool isPlaying = false;
    [SerializeField] int reward = 100;
    [SerializeField] List<GameObject> gridSlots;
    float prevTimeScale;
    Action win;
    Action lose;
    public void StartAimTrainer(Action onWin, Action onLose)
    {
        if (isPlaying) {
            throw new InvalidOperationException("Minigame already in progress");
        }
        isPlaying = true;

        panel.SetActive(true);
        prevTimeScale = Time.timeScale;
        win = onWin;
        lose = onLose;
        currentRound = 0;
        PlayerMovement.Instance.DisableMovement();
        StartCoroutine(RunAimTrainer());
    }

    private IEnumerator RunAimTrainer()
    {
        while (currentRound < roundsToWin)
        {
            SpawnTarget();
            float timer = 0f;

            while (timer < roundTime)
            {
                timer += Time.unscaledDeltaTime; // Use unscaled time since game is paused
                yield return null;

                if (currentTarget == null) // Player clicked correctly
                    break;
            }

            if (currentTarget != null) // Time ran out (didn't click)
            {
                currentTarget.SetActive(false);
                Debug.Log("Lost minigame!");
                Cleanup();
                lose();
                yield break;
            }

            currentRound++;
        }
        Debug.Log("Won minigame!");
        Cleanup();
        win();
    }

    private void SpawnTarget()
    {
        if (currentTarget != null)
            currentTarget.SetActive(false);
        currentTarget = gridSlots[Random.Range(0, gridSlots.Count)];
        currentTarget.SetActive(true);
    }

    private void Cleanup()
    {
        isPlaying = false;
        panel.SetActive(false);
        Time.timeScale = prevTimeScale;

        if (currentTarget != null)
            currentTarget.SetActive(false);
        PlayerMovement.Instance.EnableMovement();

    }
    IEnumerator HandleButtonClick(Button button)
    {
        GameObject slot = button.gameObject;
        if (!isPlaying || slot != currentTarget)
        {
            throw new InvalidOperationException("Clicked button is not the current target or game not active!");
        }
        yield return new WaitForSecondsRealtime(0.1f);
        currentTarget.SetActive(false);
        currentTarget = null;
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;
        Debug.Log("MinigameManager instance set");
        if (gridSlots == null || gridSlots.Count != 9)
        {
            throw new InvalidOperationException("MinigameManager does not have 9 grid slots assigned!");
        }
        foreach (var slot in gridSlots)
        {
            Button button = slot.GetComponent<Button>();
            button.onClick.AddListener(() => StartCoroutine(HandleButtonClick(button)));
            slot.SetActive(false);
        }

    }
    void Start()
    {
    }
}