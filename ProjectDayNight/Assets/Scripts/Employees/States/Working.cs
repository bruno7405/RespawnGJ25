using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static JobType;

public class Working : State
{
    private static readonly Dictionary<JobType, string> jobSounds = new() {
        { WaterPlant, null },
        { UsingBathroom, "ToiletFlush" },
        { ConferenceTable, null },
        { ShelvesCabinets, "FileCabinetPages" },
        { DeskWork, null },
        { Custodial, null },
        { Print, "Printer" },
    };
    
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
        if (!GameStateManager.Instance.IsDay) employee.SetNewState(employee.RunningState);
        if (employee.readyForJob)
        {
            if (RefusalRoll())
            {
                // Debug.Log(employee.Name + " refused to work due to low morale (" + employee.Morale + ")");
                employee.SetNewState(employee.SlackOffState);
                return;
            }
            StartJob();
        }
    }

    bool RefusalRoll()
    {
        float chanceToRefuse = employee.Morale switch
        {
            >= 90 => 0.1f,
            >= 70 => 0.2f,
            >= 50 => 0.3f,
            >= 20 => 0.4f,
            >= 0 => 0.5f,
            _ => 0f, // error case
        };
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
        employee.WalkTo(currentJob.Location, () =>
        {
            AudioManager.Instance.PlayOneShot(jobSounds[currentJob.Type]);
            StartCoroutine(CompleteTask());
        }, true);
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
