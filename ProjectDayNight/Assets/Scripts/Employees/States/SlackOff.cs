using UnityEngine;

public class SlackOff : State
{
    Employee employee;
    bool reachedBreakSpot;
    [SerializeField] float timeLimit = 30f;
    float timeElapsed;
    SlackOffSpot currentSpot;

    StatusIconBruno statusIcon;


    public override void OnExit()
    {
        //Debug.Log("Employee " + employee.Name + " finished slacking off");
        employee.StopMoving();
    }

    public override void OnStart()
    {
        timeLimit -= (GameStateManager.Instance.CurrentDay - 1) * 5; // Decrease time limit as days go on
        var newSpot = SlackOffSpots.TakeRandomSpot() ?? SlackOffSpots.TakeRandomSpot(true);
        if (newSpot == null) throw new System.Exception("No available slack off spots for " + employee.Name);
        currentSpot?.Unassign();
        currentSpot = newSpot;
        timeElapsed = 0f;

        employee.StateName = EmployeeState.SlackingOff;
        reachedBreakSpot = false;
        //employee.Morale += 10;  //Gain morale for idling?
        Debug.Log($"SLACK OFF --- Employee {employee.Name} will slack off at {newSpot.name}: {newSpot.Location}");
        employee.WalkTo(newSpot.Location, () => {
            statusIcon.Slacking();
            // Debug.Log("Employee " + employee.Name + " reached break spot");
            reachedBreakSpot = true;
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
