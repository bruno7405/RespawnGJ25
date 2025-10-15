using UnityEngine;

public class Escape : State
{
    Employee employee;
    Exit exit;
    static float escapeTime = 10f;
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
    }

    public override void OnStart()
    {
        if (exit == null)
        {
            throw new System.InvalidOperationException("Escape state started without an exit assigned!");
        }
        exit.Occupied = true;
        Debug.Log("Employee " + employee.Name + " is escaping");
        timer = 0f;
        employee.StateName = EmployeeState.Escaping;
        EmployeeStatusUI.Instance.UpdateUI();
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
