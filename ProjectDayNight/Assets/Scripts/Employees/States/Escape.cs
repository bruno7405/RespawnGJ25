using UnityEngine;

public class Escape : State
{
    Employee employee;
    EmployeeJobManager jobManager;
    void Awake()
    {
        employee = (Employee)stateMachine;
        jobManager = new(employee.Type.Role);
    }
    public override void OnExit()
    {
        return;
    }

    public override void OnStart()
    {
        employee.StateName = EmployeeState.Escaping;
        EmployeeStatusUI.Instance.UpdateUI();
        employee.transform.position = GridManager.WorldTileCenter(EmployeeJobManager.RandomHidePoint());
    }

    public override void OnUpdate()
    {
        return;
    }

}
