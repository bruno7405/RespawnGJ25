using System;
using UnityEngine;
public enum EmployeeState
{
    Working,
    SlackingOff,
    Sleeping,
    Escaping,
    Dead
}

public class Employee : StateMachineManager
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] string employeeName = "Ben";
    public string EmployeeName => employeeName;
    [SerializeField] EmployeeType type;
    public EmployeeType Type => type;
    [SerializeField] EmployeeMotionManager motionManager;
    [SerializeField] int wage;
    public int Wage => wage;
    [SerializeField] int revenue;
    public int Revenue => revenue;
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
    public EmployeeState StateName;
    [SerializeField] int moraleDecreasePerDay = 5;
    public int MoraleDecreasePerDay => moraleDecreasePerDay;

    /// <summary>
    /// Calculates profit made by this employee for one day
    /// </summary>
    /// <returns>Profit made by employee for one day</returns>
    public int ProfitMade()
    {
        return Sleeping ? -wage : revenue - wage;
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
            if (old == 0 && morale > 0) CompanyManager.Instance.RemoveLowMoraleEmployee(this);
            else if (morale == 0) CompanyManager.Instance.AddLowMoraleEmployee(this);
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
    public Action WalkTo(Vector2Int destination, Action callback = null)
    {
        return motionManager.WalkTo(destination, callback);
    }
    /// <summary>
    /// Makes employee run to location
    /// </summary>
    /// <param name="destination">Tilemap Coordinates</param>
    /// <param name="callback">Callback to invoke after reaching destination</param>
    /// <returns>Action to force cancel path</returns>
    public Action RunTo(Vector2Int destination, Action callback = null)
    {
        return motionManager.RunTo(destination, callback);
    }

    void OnEnable()
    {
    }
    void OnDisable()
    {
        //Use null check because this if Instance is destroyed, EmployeeList will be empty anyway
        if (CompanyManager.Instance == null) return;
        CompanyManager.Instance.UnregisterEmployee(this);
    }
    void HandleNightStart()
    {
        Morale -= moraleDecreasePerDay;
    }
    void Awake()
    {
        if (type == null)
        {
            Debug.LogError("Employee type not set for " + employeeName);
            return;
        }
        wage = type.BaseSalary;
        revenue = type.BaseRevenue;
        morale = type.BaseMorale;
        readyForJob = false;
        CompanyManager.Instance?.UpdateMorale(morale);
        moraleDecreasePerDay = UnityEngine.Random.Range(MAX_MORALE / 5, MAX_MORALE / 2);
    }

    new void Start()
    {
        base.Start();
        CompanyManager.Instance.RegisterEmployee(this);
        GameStateManager.Instance.NightStart += HandleNightStart;
    }
}
