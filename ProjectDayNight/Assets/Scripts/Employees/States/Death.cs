using UnityEngine;

public class Death : State
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
        Debug.Log("Employee has died");
        employee.StateName = EmployeeState.Dead;
        parent.SetActive(false);

        EmployeeStatusUI.Instance.UpdateUI();
    }

    public override void OnUpdate()
    {
        return;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
