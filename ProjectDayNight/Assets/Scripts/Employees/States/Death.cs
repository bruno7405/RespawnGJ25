using UnityEngine;

public class Death : State
{
    Employee employee;
    void Awake()
    {
        employee = parent.GetComponent<Employee>();
    }
    public override void OnExit()
    {
        return;
    }

    public override void OnStart()
    {
        Debug.Log("Employee has died");
        parent.SetActive(false);

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
