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
        Debug.Log("Employee " + employee.Name + " is escaping");
        employee.StateName = EmployeeState.Escaping;
        EmployeeStatusUI.Instance.UpdateUI();
        employee.transform.position = GridManager.RandomWalkablePos();
    }

    public override void OnUpdate()
    {
        return;
    }

}
