using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Working : State
{
    private Employee employee;
    private EmployeeJobManager jobManager;
    private EmployeeJob currentJob;
    private bool reachedJobSite;

    public override void OnExit()
    {
    }

    public override void OnStart()
    {
        employee.StateName = EmployeeState.Working;
        employee.readyForJob = true;
        reachedJobSite = false;

        EmployeeStatusUI.Instance.UpdateUI();
    }

    public override void OnUpdate()
    {
        if (employee.readyForJob)
        {
            if (RefusalRoll()) // Refusal Chance: 12.5% for 50-59 morale, 25% for 40-49, ... 75% for 0-9
            {
                Debug.Log(employee.EmployeeName + " refused to work due to low morale (" + employee.Morale + ")");
                employee.SetNewState(employee.SlackOffState);
                return;
            }
            StartJob();
        }
        else if (reachedJobSite)
        {
            StartCoroutine(CompleteTask());
        }
    }

    bool RefusalRoll()
    {
        float chanceToRefuse = Mathf.Max(6 - employee.Morale / 10, 0) * 0.125f; // Increases by 12.5% for every 10 morale below 60, max 75%
        return Random.value < chanceToRefuse;
    }

    void StartJob()
    {
        employee.readyForJob = false;
        employee.Morale -= 10; // Lose morale for each task?
        currentJob = jobManager.NewJob();

        Debug.Log("Starting job at " + currentJob.location + " for " + currentJob.duration + " seconds");
        employee.WalkTo(currentJob.location, () => reachedJobSite = true);
    }

    IEnumerator CompleteTask()
    {
        reachedJobSite = false;
        yield return new WaitForSeconds(currentJob.duration);
        employee.readyForJob = true;
    }

    void Awake()
    {
        employee = (Employee)stateMachine;
        jobManager = new(employee.Type.Role);
    }
}
