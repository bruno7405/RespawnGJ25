using UnityEngine;

public class Working : State
{
    Employee employee;

    public override void OnExit()
    {
    }

    public override void OnStart()
    {
        employee.StartWorking();
    }

    public override void OnUpdate()
    {
    }

    void Awake() 
    {
        employee = (Employee)stateMachine;
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
