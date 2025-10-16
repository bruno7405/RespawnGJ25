using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DayState : State
{
    float duration;
    float timeElapsed;

    [Header("Parameters")]
    [SerializeField] float timeBetweenTasks;
    private float taskTimer = 0;
    private int taskCount = 0;
    private int dialogTaskCount = 0;

    [Header("DialogTasks")]
    [SerializeField] List<DialogEvent> dialogEvents = new List<DialogEvent>();



    [Header("References")]
    [SerializeField] State nightState;
    [SerializeField] TimeUI timeUI;
    [SerializeField] BossTaskManagerBruno bossTaskManager;

    public override void OnStart()
    {
        timeBetweenTasks = duration / (2 + (GameStateManager.Instance.CurrentDay * 2)); // More tasks as days go on
        // Visuals
        // Show day number
        // Invoke events in random time?
        Debug.Log("day state");
        ((GameStateManager)stateMachine).InvokeNewDay();

        // UI Popup
        InformationPopupUI.Instance.DisplayText("Keep employees on task", true);

        // Play Music
        AudioManager.Instance.PlayBackgroundMusic("DaySong");

        // Lights
        VisualsManager.Instance.LightsOn();

        // Reset task counts
        taskCount = 0;
        dialogTaskCount = 0;

        timeElapsed = 0;
    }

    public override void OnUpdate()
    {
        timeElapsed += Time.deltaTime;
        // timeUI.SetTimeElasped(timeElapsed, true);
        
        if (timeElapsed >= duration)
        {
            stateMachine.SetNewState(nightState);
        }

        // Task activating
        taskTimer += Time.deltaTime;
        if (taskTimer >= timeBetweenTasks)
        {
            if (taskCount % 3 == 2) // every 3 tasks is a dialogEventTask
            {
                SetDialogTask();
                dialogTaskCount++;
            }
            else
            {
                bossTaskManager.ActivateRandomTask();
            }
            taskCount++;
            taskTimer = 0;
        }
    }

    public override void OnExit()
    {
        return;
    }

    public void SetDuration(float d)
    {
        duration = d;
    }

    private void SetDialogTask()
    {
        if (dialogTaskCount == 0) // Lore dialog, first dialog event to happen
        {
            bossTaskManager.ActivateDialogTask(dialogEvents[GameStateManager.Instance.CurrentDay - 1]);
        }
        else // Random dialog
        {
            bossTaskManager.ActivateDialogTask(dialogEvents[Random.Range(3, dialogEvents.Count)]);
        }
        
    }
}
