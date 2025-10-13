using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CompanyManager : MonoBehaviour
{
    public static CompanyManager Instance { get; private set; }
    [SerializeField] int startingMoney = 100;
    [SerializeField] int startingNumEmployees = 5;
    public int Money { get; private set; }
    public int Morale { get; private set; }
    public int NumEmployees { get; private set; }
    HashSet<Employee> employees = new();
    HashSet<Employee> lowMoraleEmployees = new();
    void HandleNightStart()
    {
        AddProfit();

        // Calculate # Escapists
        int numEscapists = employees.Count - Mathf.Max(Morale - 1, 0) * employees.Count / 100;
        employees.OrderBy(e => e.Morale).Take(numEscapists).ToList().ForEach(e => e.SetNewState(e.EscapeState));
    }
    void HandleDayStart()
    {
        foreach (var emp in employees)
        {
            emp.SetNewState(emp.WorkingState);
        }
    }
    public void RegisterEmployee(Employee e)
    {
        employees.Add(e);
        NumEmployees++;

    }
    public void UnregisterEmployee(Employee e)
    {
        if (employees.Remove(e))
        {
            NumEmployees--;
        }
        lowMoraleEmployees.Remove(e);
    }
    public void AddLowMoraleEmployee(Employee e)
    {
        lowMoraleEmployees.Add(e);
    }
    public void RemoveLowMoraleEmployee(Employee e)
    {
        lowMoraleEmployees.Remove(e);
    }
    public Employee[] GetEmployeeList()
    {
        return employees.ToArray();
    }
    public Employee[] GetLowMoraleEmployees()
    {
        return lowMoraleEmployees.ToArray();
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
    public void ChangeCompanyMorale(int delta)
    {
        foreach (var emp in employees)
        {
            emp.Morale += delta;
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
        GameStateManager.Instance.NewDay += HandleDayStart;
        GameStateManager.Instance.NightStart += HandleNightStart;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
