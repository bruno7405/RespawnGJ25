using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Working : State
{
    private Employee employee;
    private EmployeeJob currentJob;
    private Action cancelWalk;

    public override void OnExit()
    {
        cancelWalk?.Invoke();
    }

    public override void OnStart()
    {
        employee.StateName = EmployeeState.Working;
        employee.readyForJob = true;

        EmployeeStatusUI.Instance.UpdateUI();
    }

    public override void OnUpdate()
    {
        if (employee.readyForJob)
        {
            if (RefusalRoll()) // Refusal Chance: 12.5% for 50-59 morale, 25% for 40-49, ... 75% for 0-9
            {
                Debug.Log(employee.Name + " refused to work due to low morale (" + employee.Morale + ")");
                employee.SetNewState(employee.SlackOffState);
                return;
            }
            StartJob();
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
        var newJob = employee.GetNewJob();
        currentJob?.Unassign();

        if (newJob == null)
        {
            Debug.Log(employee.Name + " has no available jobs, going to slack off");
            employee.SetNewState(employee.SlackOffState);
            return;
        }

        currentJob = newJob;
        Debug.Log("Employee " + employee.Name + " starting " + currentJob.Name + " at " + currentJob.Location + " for " + currentJob.Duration + " seconds");
        cancelWalk = employee.WalkTo(currentJob.Location, () => StartCoroutine(CompleteTask()));
    }

    IEnumerator CompleteTask()
    {
        yield return new WaitForSeconds(currentJob.Duration);
        employee.readyForJob = true;
    }

    void Awake()
    {
        employee = (Employee)stateMachine;
    }
}
