using UnityEngine;

public class SlackOff : State
{
    Employee employee;
    bool reachedBreakSpot;
    static float timeLimit = 10f;
    float timeElapsed;

    public override void OnExit()
    {
        Debug.Log("Employee " + employee.Name + " finished slacking off");
        employee.StatusIcon.Hide();
    }

    public override void OnStart()
    {
        employee.StatusIcon.SetSprite(employee.StatusIcon.SlackingOffSprite);
        employee.StateName = EmployeeState.SlackingOff;
        reachedBreakSpot = false;
        //employee.Morale += 10;  //Gain morale for idling?
        employee.WalkTo(new(3, 4), () => {
            Debug.Log("Employee " + employee.Name + " reached break spot");
            reachedBreakSpot = true;
            employee.StatusIcon.Show();
        });

        EmployeeStatusUI.Instance.UpdateUI();
    }

    public override void OnUpdate()

    {
        if (reachedBreakSpot)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeLimit)
            {
                employee.SetNewState(employee.SleepingState);
            }
        }
    }

    void Awake()
    {
        employee = (Employee)stateMachine;
    }
}
