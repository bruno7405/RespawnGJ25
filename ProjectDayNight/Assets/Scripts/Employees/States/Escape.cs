using UnityEngine;

public class Escape : State
{
    Employee employee;
    void Awake()
    {
        employee = (Employee)stateMachine;
    }
    public override void OnExit()
    {
        return;
    }

    public override void OnStart()
    {
        Debug.Log("Employee is escaping");
        employee.StateName = EmployeeState.Escaping;

    }

    public override void OnUpdate()
    {
        return;
    }

}
