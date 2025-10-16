using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
public enum EmployeeState
{
    Working,
    SlackingOff,
    Sleeping,
    Escaping,
    Running,
    Dead
}

public class Employee : StateMachineManager
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] string employeeName = "Ben";
    public string Name => employeeName;
    [SerializeField] EmployeeType type;
    public EmployeeType Type => type;
    public Role Role => type.Role;
    [SerializeField] EmployeeMotionManager motionManager;
    [SerializeField] int wage;
    public int Wage => wage;
    public int Revenue;
    [SerializeField] int morale;
    public const int MAX_MORALE = 100;
    public bool readyForJob;
    public bool Sleeping = false;

    //States
    [SerializeField] Death deathState;
    public Death DeathState => deathState;
    [SerializeField] SlackOff slackOffState;
    public SlackOff SlackOffState => slackOffState;
    [SerializeField] Working workingState;
    public Working WorkingState => workingState;
    [SerializeField] Escape escapeState;
    public Escape EscapeState => escapeState;
    [SerializeField] Sleeping sleepingState;
    public Sleeping SleepingState => sleepingState;
    [SerializeField] Running runningState;
    public Running RunningState => runningState;
    public EmployeeState StateName;
    [SerializeField] int moraleDecreasePerDay = 5;
    public int MoraleDecreasePerDay => moraleDecreasePerDay;

    [SerializeField] private StatusIcon statusIcon;
    public StatusIcon StatusIcon => statusIcon;

    /// <summary>
    /// Calculates profit made by this employee for one day
    /// </summary>
    /// <returns>Profit made by employee for one day</returns>
    public int ProfitMade()
    {
        return Sleeping ? -wage : Revenue - wage;
    }
    public void KillEmployee()
    {
        SetNewState(deathState);
    }
    public void UrgeWork()
    {
        SetNewState(workingState);
    }

    public int Morale
    {
        get => morale;
        set
        {
            int old = morale;
            morale = Mathf.Clamp(value, 0, MAX_MORALE);
            //don't use null check because we want to know if it fails
            CompanyManager.Instance.UpdateMorale(morale - old);
        }
    }


    // Motion Properties & Methods
    public float WalkSpeed => motionManager.WalkSpeed;
    public float RunSpeed => motionManager.RunSpeed;
    /// <summary>
    /// Makes employee walk to location
    /// </summary>
    /// <param name="destination">Tilemap Coordinates</param>
    /// <param name="callback">Callback to invoke after reaching destination</param>
    /// <returns>Action to force cancel path</returns>
    public Action WalkTo(Vector2 destination, Action callback = null)
    {
        return motionManager.WalkTo(destination, callback);
    }
    /// <summary>
    /// Makes employee run to location
    /// </summary>
    /// <param name="destination">Tilemap Coordinates</param>
    /// <param name="callback">Callback to invoke after reaching destination</param>
    /// <returns>Action to force cancel path</returns>
    public Action RunTo(Vector2 destination, Action callback = null)
    {
        return motionManager.RunTo(destination, callback);
    }
    public void StopMoving()
    {
        motionManager.Stop();
    }

    void OnEnable()
    {
        if (GameStateManager.Instance != null) GameStateManager.Instance.NightStart += HandleNightStart;
    }
    void OnDisable()
    {
        //Use null check because this if Instance is destroyed, EmployeeList will be empty anyway
        currentState?.OnExit();
        if (GameStateManager.Instance != null) GameStateManager.Instance.NightStart -= HandleNightStart;
        if (CompanyManager.Instance == null) return;
        CompanyManager.Instance.UnregisterEmployee(this);
    }
    void HandleNightStart()
    {
        Morale -= moraleDecreasePerDay;
    }
    public void StartEscape(Exit exit)
    {
        EscapeState.SetExit(exit);
        currentState?.OnExit();
        currentState = EscapeState;
        currentState.OnStart();
    }
    public override void SetNewState(State newState)
    {
        if (StateName == EmployeeState.Dead)
        {
            Debug.LogWarning("Tried to change state of dead employee " + Name);
        }
        if (newState == escapeState)
        {
            throw new ArgumentException("Use StartEscape(Exit) to set Escape state");
        }
        base.SetNewState(newState);
    }
    void Awake()
    {
        if (type == null)
        {
            Debug.LogError("Employee type not set for " + employeeName);
            return;
        }
        gameObject.name = "Employee_" + employeeName;
        wage = type.BaseSalary;
        Revenue = type.BaseRevenue;
        morale = type.BaseMorale;
        readyForJob = false;
        CompanyManager.Instance?.UpdateMorale(morale);
        moraleDecreasePerDay = Random.Range(MAX_MORALE / 5, MAX_MORALE / 2);
    }

    new void Start()
    {
        base.Start();
        CompanyManager.Instance.RegisterEmployee(this);
        GameStateManager.Instance.DayStart -= HandleNightStart;
        GameStateManager.Instance.NightStart += HandleNightStart;
    }

}
