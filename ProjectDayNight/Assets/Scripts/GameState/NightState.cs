using UnityEngine;

public class NightState : State
{
    float duration;
    float timeElapsed;

    [SerializeField] State endState;
    [SerializeField] TimeUI timeUI;
    public override void OnStart()
    {
        // Call EmployeeManager to start escape sequence
        Debug.Log("night state");

        timeElapsed = 0;
    }

    public override void OnUpdate()
    {
        timeElapsed += Time.deltaTime;
        timeUI.SetTimeElasped(timeElapsed, false);

        if (timeElapsed >= duration)
        {
            stateMachine.SetNewState(endState);
        }
    }

    public void SetDuration(float d)
    {
        duration = d;
    }
    public override void OnExit()
    {
        return;
    }
}
