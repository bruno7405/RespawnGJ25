using UnityEngine;

public class Running : State
{
    Employee employee;
    void Awake()
    {
        employee = (Employee)stateMachine;
    }
    public override void OnExit()
    {
        CompanyManager.Instance.RemoveRunner(employee);
        employee.StopMoving();
    }

    public override void OnStart()
    {
        employee.StateName = EmployeeState.Running;
        CompanyManager.Instance.AddRunner(employee);
        EmployeeStatusUI.Instance.UpdateUI();
        RunLoop();
    }
    void RunLoop()
    {
        //Debug.Log($"RunLoop called on: {this}  (emp is: {employee})");
        employee.RunTo(GridManager.RandomWalkablePos(), RunLoop);
    }

    public override void OnUpdate()
    {
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
