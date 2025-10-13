using UnityEngine;

public class EmployeeStatusUI : MonoBehaviour
{
    [SerializeField] Transform cardParent;
    [SerializeField] GameObject employeeStatusCard;
    Employee[] employees;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (Transform child in cardParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        employees = CompanyManager.Instance.GetEmployeeList();
        foreach (Employee employee in employees)
        {
            var card = Instantiate(employeeStatusCard, cardParent);
            card.GetComponent<EmployeeCardUI>().UpdateCardUI(employee);
        }
    }
}
