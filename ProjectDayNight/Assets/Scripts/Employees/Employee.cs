using Unity.VisualScripting;
using UnityEngine;

public class Employee : StateMachineManager
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] string employeeName = "Ben";
    [SerializeField] EmployeeType type;
    [SerializeField] int wage;
    [SerializeField] int revenue;
    [SerializeField] int morale;
    public const int MAX_MORALE = 100;


    //States
    [SerializeField] Death deathState;
    [SerializeField] Idle idleState;
    /// <summary>
    /// Calculates profit made by this employee for one day
    /// </summary>
    /// <returns>Profit made by employee for one day</returns>
    public int ProfitMade()
    {
        if (Working)
        {
            return revenue - wage;
        }
        return -wage;
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
    private bool Working = true;
    public void StartWorking()
    {
        Working = true;
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
        CompanyManager.Instance?.UpdateMorale(morale);
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }



}
