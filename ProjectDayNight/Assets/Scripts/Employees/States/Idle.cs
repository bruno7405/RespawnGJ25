using UnityEngine;

public class Idle : State
{
    Employee employee;
    bool reachedBreakSpot;
    
    public override void OnExit()
    {
    }

    public override void OnStart()
    {
        reachedBreakSpot = false;
        employee.Morale += 10; // Gain morale for idling?
        employee.WalkTo(new(0,9), () => reachedBreakSpot = true);
    }

    public override void OnUpdate()
    {
    }

    void Awake()
    {
        employee = (Employee)stateMachine;
    }
}
