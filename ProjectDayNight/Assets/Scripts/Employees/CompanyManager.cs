using UnityEngine;

public class CompanyManager : MonoBehaviour
{
    public static CompanyManager Instance { get; private set; }

    [SerializeField] int startingMoney = 100;
    [SerializeField] int startingNumEmployees = 5;

    public int Money { get; private set; }
    public int Morale { get; private set; }
    public int NumEmployees { get; private set; }
    public Employee[] Employees { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ensure one active per scene
            return;
        }
        Instance = this;

        Money = startingMoney;
        NumEmployees = startingNumEmployees;
    }

    public bool SpendMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Cannot spend negative money");
            return false;
        }
        if (amount <= Money)
        {
            Money -= amount;
            return true;
        }
        return false; // not enough money
    }
    public void RecalculateMorale()
    {
        if (Employees.Length == 0)
        {
            Morale = 0;
            return;
        }

        int totalMorale = 0;
        foreach (var emp in Employees)
        {
            totalMorale += emp.Morale;
        }
        Morale = totalMorale / NumEmployees; // average morale
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
