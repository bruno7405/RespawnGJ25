using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Working : State
{
    private Employee employee;
    private EmployeeJob currentJob;
    StatusIconBruno statusIcon;
    public override void OnExit()
    {
        employee.StopMoving();
    }

    public override void OnStart()
    {
        employee.StateName = EmployeeState.Working;
        employee.readyForJob = true;

        EmployeeStatusUI.Instance.UpdateUI();

        // ICON
        if (employee.Morale < 33) statusIcon.LowMorale();
        else if (employee.Morale < 67)  statusIcon.MediumMorale();
        else statusIcon.HighMorale();
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
        float chanceToRefuse = Mathf.Max(6 - employee.Morale / 10, 0) * 0.125f;
        return Random.value < chanceToRefuse;
    }

    void StartJob()
    {
        employee.readyForJob = false;
        var newJob = EmployeeJobRegistry.TakeRandomJob(employee.Role);
        currentJob?.Unassign();

        if (newJob == null)
        {
            Debug.Log(employee.Name + " has no available jobs, going to slack off");
            employee.SetNewState(employee.SlackOffState);
            return;
        }

        currentJob = newJob;
        //Debug.Log("Employee " + employee.Name + " starting " + currentJob.Name + " at " + currentJob.Location + " for " + currentJob.Duration + " seconds");
        employee.WalkTo(currentJob.Location, () => StartCoroutine(CompleteTask()), true);
    }

    IEnumerator CompleteTask()
    {
        yield return new WaitForSeconds(currentJob.Duration);
        employee.readyForJob = true;
    }

    void Awake()
    {
        employee = (Employee)stateMachine;
        statusIcon = GetComponentInChildren<StatusIconBruno>();
    }
}
