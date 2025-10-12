using System;
using UnityEngine;

public class Employee : StateMachineManager
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] string employeeName = "Ben";
    public string EmployeeName => employeeName;
    [SerializeField] EmployeeType type;
    public EmployeeType Type => type;
    [SerializeField] EmployeeMotionManager motionManager;
    [SerializeField] int wage;
    [SerializeField] int revenue;
    [SerializeField] int morale;
    public const int MAX_MORALE = 100;
    public bool readyForJob;
    private bool Working = true;

    //States
    [SerializeField] Death deathState;
    public Death DeathState => deathState;
    [SerializeField] Idle idleState;
    public Idle IdleState => idleState;
    [SerializeField] Working workingState;
    public Working WorkingState => workingState;
    /// <summary>
    /// Calculates profit made by this employee for one day
    /// </summary>
    /// <returns>Profit made by employee for one day</returns>
    public int ProfitMade()
    {
        return Working ? revenue - wage : -wage;
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
            if (old == 0 && morale > 0) CompanyManager.Instance.AddLowMoraleEmployee(this);
            else if (morale == 0) CompanyManager.Instance.RemoveLowMoraleEmployee(this);
        }
    }
    public void StartWorking()
    {
        Working = true;
        readyForJob = true;
    }

    // Motion Properties & Methods
    public float WalkSpeed => motionManager.WalkSpeed;
    public float RunSpeed => motionManager.RunSpeed;
    /// <summary>
    /// Makes employee walk to location
    /// </summary>
    /// <param name="destination">Tilemap Coordinates</param>
    /// <param name="callback">Callback to invoke after reaching destination</param>
    public void WalkTo(Vector2Int destination, Action callback = null)
    {
        motionManager.WalkTo(destination, callback);
    }
    /// <summary>
    /// Makes employee run to location
    /// </summary>
    /// <param name="destination">Tilemap Coordinates</param>
    /// <param name="callback">Callback to invoke after reaching destination</param>
    public void RunTo(Vector2Int destination, Action callback = null)
    {
        motionManager.RunTo(destination, callback);
    }

    void OnEnable()
    {
        //don't use null check because we want to know if it fails
        CompanyManager.Instance.RegisterEmployee(this);

    }
    void OnDisable()
    {
        //Use null check because this if Instance is destroyed, EmployeeList will be empty anyway
        if (CompanyManager.Instance == null) return;
        CompanyManager.Instance?.UnregisterEmployee(this);
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
    }
}
