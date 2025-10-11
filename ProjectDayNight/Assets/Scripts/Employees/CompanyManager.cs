using UnityEngine;

public class CompanyManager : MonoBehaviour
{
    public static CompanyManager Instance { get; private set; }

    [SerializeField] int startingMoney = 100;
    [SerializeField] int startingMorale = 100;
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
        Morale = startingMorale;
        NumEmployees = startingNumEmployees;
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= Money)
        {
            Money -= amount;
            return true;
        }
        return false; // not enough money
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
