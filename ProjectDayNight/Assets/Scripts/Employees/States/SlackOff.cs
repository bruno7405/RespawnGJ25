using UnityEngine;

public class SlackOff : State
{
    Employee employee;
    bool reachedBreakSpot;
    static float timeLimit = 10f;
    float timeElapsed;
    SlackOffSpot currentSpot;

    StatusIconBruno statusIcon;


    public override void OnExit()
    {
        //Debug.Log("Employee " + employee.Name + " finished slacking off");
        employee.StopMoving();
        employee.StatusIcon.Hide();
    }

    public override void OnStart()
    {
        statusIcon.Slacking();
        var newSpot = SlackOffSpots.TakeRandomSpot() ?? SlackOffSpots.TakeRandomSpot(true);
        if (newSpot == null) throw new System.Exception("No available slack off spots for " + employee.Name);
        currentSpot?.Unassign();
        currentSpot = newSpot;

        employee.StatusIcon.SetSprite(employee.StatusIcon.SlackingOffSprite);
        employee.StateName = EmployeeState.SlackingOff;
        reachedBreakSpot = false;
        //employee.Morale += 10;  //Gain morale for idling?
        Debug.Log($"SLACK OFF --- Employee {employee.Name} will slack off at {newSpot.name}: {newSpot.Location}");
        employee.WalkTo(newSpot.Location, () => {
            Debug.Log("Employee " + employee.Name + " reached break spot");
            reachedBreakSpot = true;
            employee.StatusIcon.Show();
        }, true);

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
        statusIcon = GetComponentInChildren<StatusIconBruno>();
    }
}
