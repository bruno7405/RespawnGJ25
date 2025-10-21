using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Collections;
using System.Collections;

public class Exit
{
    public Vector2 position;
    public bool Occupied;
}
public class CompanyManager : MonoBehaviour
{
    public static CompanyManager Instance { get; private set; }
    [SerializeField] int startingMoney = 100;
    [SerializeField] int startingNumEmployees = 5;
    public int Money { get; private set; }
    public int Morale { get; private set; }
    public int NumEmployees { get; private set; }
    [SerializeField] int minEscapists = 3;
    HashSet<Employee> employees = new();
    HashSet<Employee> runningEmployees = new();
    [SerializeField] int numExits = 2;
    [SerializeField] Transform[] exitPositions;
    Exit[] exits;
    public int NumRunners;
    [SerializeField] float minEscapeInterval = 10f;
    [SerializeField] float maxEscapeInterval = 30f;
    public bool Escaping = false;
    [SerializeField] List<Transform> deskPositions = new();

    [SerializeField] List<GameObject> employeePrefabs;

    //Day stats
    public int MistakesToday;
    public int MoraleGained;

    void HandleNightStart()
    {
        AddProfit();
        // Calculate # Escapists
        if (minEscapists > NumEmployees)
            throw new System.InvalidOperationException("minEscapists cannot be greater than startingNumEmployees!");
        NumRunners = Mathf.Max(employees.Count - Morale * employees.Count / 100, minEscapists);
        List<Employee> emps = new(employees.OrderBy(e => e.Morale));
        runningEmployees = new();
        int count = 0;
        foreach (var emp in emps)
        {
            if (count < NumRunners)
            {
                emp.SetNewState(emp.RunningState);
                runningEmployees.Add(emp);
            }
            else
            {
                if (deskPositions.Count <= count - NumRunners) continue;
                emp.transform.position = deskPositions[count - NumRunners].position;
                emp.SetNewState(emp.SleepingState);
            }
            count++;
        }
        StartCoroutine(EscapeLoop());
    }
    IEnumerator EscapeLoop()
    {
        if (GameStateManager.Instance.currentState != GameStateManager.Instance.NightState)
        {
            yield break; // only run at night
        }
        while (NumRunners > 0)
        {
            // 1. Wait random time
            float waitTime = Random.Range(minEscapeInterval, maxEscapeInterval);
            yield return new WaitForSeconds(waitTime);

            if (NumRunners <= 0) yield break;
            if (Escaping) continue;
            AssignEmployeeToExit();
        }
    }
    public void AssignEmployeeToExit()
    {
        if (runningEmployees.Count == 0) return;
        Employee emp = runningEmployees.First();
        Exit exit = exits[Random.Range(0, numExits)];
        emp.RunTo(exit.position, () => emp.StartEscape(exit));
    }
    void HandleDayStart()
    {
        // For all employee candidates, respawn ones that are dead
        if (GameStateManager.Instance.CurrentDay != 1)
        {
            foreach (var employeePrefab in employeePrefabs)
            {
                var candidate = employeePrefab.GetComponent<Employee>();
                bool skipEmployee = false;
                foreach (var e in employees) // check if candidate is already alive in company
                {
                    if (candidate.name.Equals(e.name))
                    {
                        skipEmployee = true;
                    }
                }

                if (skipEmployee) continue;
                else
                {
                    // Debug.Log("Respawning: " + candidate.name);
                    Instantiate(employeePrefab, GridManager.RandomWalkablePos(), Quaternion.identity);
                }
            }
        }
        MistakesToday = 0;
        MoraleGained = 0;

        // Set employees to work
        foreach (var emp in employees)
        {
            emp.SetNewState(emp.WorkingState);
        }
    }
    public void AddRunner(Employee e)
    {
        runningEmployees.Add(e);
    }
    public void RemoveRunner(Employee e)
    {
        runningEmployees.Remove(e);
    }
    public void RegisterEmployee(Employee e)
    {
        if (employees.Add(e)) NumEmployees++;
    }
    public void UnregisterEmployee(Employee e)
    {
        if (employees.Remove(e)) NumEmployees--;
    }
    public Employee[] GetEmployeeList()
    {
        return employees.ToArray();
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
        PlayerStatsUI.Instance.SetMoney(Money);
        return false; // not enough money
    }
    public bool AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Cannot add negative money");
            return false;
        }
        Money += amount;
        PlayerStatsUI.Instance.SetMoney(Money);
        return true;
    }
    public void ChangeCompanyMorale(int delta)
    {
        foreach (var emp in employees)
        {
            emp.Morale += delta;
        }
    }
    public void ChangeCompanyRevenue(int pct)
    {
        foreach (var emp in employees)
        {
            emp.Revenue += emp.Revenue * pct / 100;
        }
    }
    public int GetProfit()
    {
        int totalProfit = 0;
        foreach (var emp in employees)
        {
            totalProfit += emp.ProfitMade();
        }
        return totalProfit;
    }
    public void AddProfit()
    {
        Money += GetProfit();
        PlayerStatsUI.Instance.SetMoney(Money);

        if (Money < 0)
        {
            GameStateManager.Instance.GameOver();
        }
    }
    public void UpdateMorale(int delta)
    {
        if (NumEmployees == 0)
        {
            Morale = 0;
            return;
        }
        Morale += delta / NumEmployees; // average morale
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple CompanyManager instances detected! Destroying duplicate.");
            Destroy(gameObject); // ensure one active per scene
            return;
        }
        Debug.Log("CompanyManager instance set");
        Instance = this;

        Money = startingMoney;
        NumEmployees = startingNumEmployees;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameStateManager.Instance.DayStart -= HandleDayStart;
        GameStateManager.Instance.DayStart += HandleDayStart;

        GameStateManager.Instance.NightStart -= HandleNightStart;
        GameStateManager.Instance.NightStart += HandleNightStart;

        exits = new Exit[numExits];
        if (exitPositions.Length < numExits)
        {
            throw new System.InvalidOperationException("Not enough exit positions set in CompanyManager!");
        }
        for (int i = 0; i < numExits; i++)
        {
            exits[i] = new Exit { position = exitPositions[i].position, Occupied = false };
        }
    }
    void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.DayStart += HandleDayStart;
            GameStateManager.Instance.NightStart += HandleNightStart;
        }
    }
    void OnDisable()
    {
        GameStateManager.Instance.DayStart -= HandleDayStart;
        GameStateManager.Instance.NightStart -= HandleNightStart;
    }

    // Update is called once per frame
    void Update()
    {

    }
}