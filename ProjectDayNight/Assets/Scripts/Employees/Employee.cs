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
    public bool Working { get; private set; } = true;
    public const int MAX_MORALE = 100;

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
    public int ProfitMade()
    {
        if (Working)
        {
            return revenue - wage;
        }
        return -wage;
    }


}
