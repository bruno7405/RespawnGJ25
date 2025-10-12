using System;
using UnityEngine;

public class DayState : State
{
    float duration;
    float timeElapsed;

    [SerializeField] State nightState;
    [SerializeField] TimeUI timeUI;
    public override void OnStart()
    {
        // Visuals
        // Show day number
        // Invoke events in random time?
        Debug.Log("day state");

        // Coroutine for transition screen/animation?

        Array.ForEach(CompanyManager.Instance.GetEmployeeList(), e => e.SetNewState(e.WorkingState));
        timeElapsed = 0;
    }

    public override void OnUpdate()
    {
        timeElapsed += Time.deltaTime;
        timeUI.SetTimeElasped(timeElapsed, true);

        if (timeElapsed >= duration)
        {
            CompanyManager.Instance.AddProfit();
            stateMachine.SetNewState(nightState);
        }
    }
    public override void OnExit()
    {
        return;
    }

    public void SetDuration(float d) => duration = d;
}
