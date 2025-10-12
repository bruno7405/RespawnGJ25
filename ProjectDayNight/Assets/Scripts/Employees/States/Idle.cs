using UnityEngine;

public class Idle : State
{
    Employee employee;
    void Awake()
    {
        employee = (Employee)stateMachine;
    }
    public override void OnExit()
    {
    }

    public override void OnStart()
    {
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
