using UnityEngine;

public class Escape : State
{
    Employee employee;
    Exit exit;
    static float escapeTime = 30f;
    float timer = 0f;

    public void SetExit(Exit exit)
    {
        this.exit = exit;
    }
    void Awake()
    {
        employee = (Employee)stateMachine;
    }
    public override void OnExit()
    {
        exit.Occupied = false;
        CompanyManager.Instance.Escaping = false;

    }

    public override void OnStart()
    {
        if (exit == null)
        {
            throw new System.InvalidOperationException("Escape state started without an exit assigned!");
        }
        if (CompanyManager.Instance.Escaping)
        {
            throw new System.InvalidOperationException("Another employee is already escaping!");
        }
        CompanyManager.Instance.Escaping = true;
        exit.Occupied = true;
        Debug.Log("Employee " + employee.Name + " is escaping");
        timer = 0f;
        employee.StateName = EmployeeState.Escaping;
        EmployeeStatusUI.Instance.UpdateUI();
        InformationPopupUI.Instance.DisplayText(employee.Name + " is at the exit. Stop them before they escape", false, 2f);
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= escapeTime)
        {
            GameStateManager.Instance.GameOver();
        }

        return;
    }

}
