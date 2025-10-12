using UnityEngine;

public class SlackOff : State
{
    Employee employee;
    bool reachedBreakSpot;
    static float timeLimit = 10f;
    float timeElapsed;

    public override void OnExit()
    {
    }

    public override void OnStart()
    {
        reachedBreakSpot = false;
        //employee.Morale += 10;  //Gain morale for idling?
        employee.WalkTo(new(0,9), () => reachedBreakSpot = true);
    }

    public override void OnUpdate()

    {
        if (reachedBreakSpot)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeLimit)
            {
                employee.SetNewState(employee.SleepingState);
            }
        }
    }

    void Awake()
    {
        employee = (Employee)stateMachine;
    }
}
