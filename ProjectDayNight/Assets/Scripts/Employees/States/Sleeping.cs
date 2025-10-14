public class Sleeping : State
{
    Employee employee;

    void Awake()
    {
        employee = (Employee)stateMachine;
    }
    public override void OnExit()
    {
        
        employee.Sleeping = false;
    }

    public override void OnStart()
    {
        employee.Sleeping = true;
        employee.StateName = EmployeeState.Sleeping;
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
