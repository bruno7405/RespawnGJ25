using UnityEngine;

public class Employee : StateMachineManager
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] string employeeName = "Ben";
    [SerializeField] EmployeeType type;
    public EmployeeType Type => type;
    [SerializeField] int wage;
    [SerializeField] int revenue;
    [SerializeField] int morale;
    public float moveSpeed;
    public bool readyForJob;
    public const int MAX_MORALE = 100;
    private bool Working = true;


    //States
    [SerializeField] Death deathState;
    public Death DeathState => deathState;
    [SerializeField] Idle idleState;
    public Idle IdleState => idleState;
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
