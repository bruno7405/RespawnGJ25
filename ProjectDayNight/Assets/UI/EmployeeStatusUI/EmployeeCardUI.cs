using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeCardUI : MonoBehaviour
{
    [SerializeField] Image employeeIcon;
    [SerializeField] TextMeshProUGUI employeeName;
    [SerializeField] TextMeshProUGUI employeeMoney;
    [SerializeField] TextMeshProUGUI status;

    [SerializeField] Color normalColor;
    [SerializeField] Color badColor;
    [SerializeField] Color sleepColor;

    public void UpdateCardUI(Employee employee)
    {
        employeeIcon.sprite = employee.GetComponentInChildren<SpriteRenderer>().sprite;
        employeeName.text = employee.EmployeeName;
        employeeMoney.text = "Wage: $" + employee.Wage + "\nMakes: $" + employee.Revenue;
        
        string stateName = employee.StateName.ToString();
        
        if (employee.StateName == EmployeeState.Dead)
        {
            SemiTransparent();
            status.color = Color.gray;
        }
        else if (employee.StateName == EmployeeState.Escaping || employee.StateName == EmployeeState.SlackingOff)
        {
            Opaque();
            status.color = badColor;
        }
        else if (employee.StateName == EmployeeState.Sleeping)
        {
            SemiTransparent();
            status.color = sleepColor;
        }
        else // employee in working state
        {
            Opaque();
            status.color = normalColor;
        }

        status.text = employee.StateName.ToString().ToUpper();
    }

    private void Opaque()
    {
        employeeIcon.color = new Color(employeeIcon.color.r, employeeIcon.color.g, employeeIcon.color.b, 1f);
        employeeName.color = new Color(employeeName.color.r, employeeName.color.g, employeeName.color.b, 1f);
        employeeMoney.color = new Color(employeeMoney.color.r, employeeMoney.color.g, employeeMoney.color.b, 1f);
        status.color = new Color(status.color.r, status.color.g, status.color.b, 1f);
    }

    private void SemiTransparent()
    {
        employeeIcon.color = new Color(employeeIcon.color.r, employeeIcon.color.g, employeeIcon.color.b, 0.5f);
        employeeName.color = new Color(employeeName.color.r, employeeName.color.g, employeeName.color.b, 0.5f);
        employeeMoney.color = new Color(employeeMoney.color.r, employeeMoney.color.g, employeeMoney.color.b, 0.5f);
        status.color = new Color(status.color.r, status.color.g, status.color.b, 0.5f);
    }
}
