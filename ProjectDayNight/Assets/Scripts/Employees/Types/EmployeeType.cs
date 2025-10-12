using UnityEngine;


public enum Role
{
    Intern,
    Employee,
    Manager
}
[CreateAssetMenu(fileName = "EmployeeType", menuName = "Scriptable Objects/EmployeeType")]
public class EmployeeType : ScriptableObject
{
    [SerializeField] Role role;
    [SerializeField] int baseSalary;
    [SerializeField] int baseRevenue;
    [SerializeField] int baseMorale;
    public int BaseSalary => baseSalary;
    public int BaseRevenue => baseRevenue;
    public int BaseMorale => baseMorale;
}

