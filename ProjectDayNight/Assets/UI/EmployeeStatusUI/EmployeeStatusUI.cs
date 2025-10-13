using System;
using UnityEngine;

public class EmployeeStatusUI : MonoBehaviour
{
    private static EmployeeStatusUI instance;
    public static EmployeeStatusUI Instance => instance;

    [SerializeField] GameObject uiParent;
    [SerializeField] Transform cardParent;
    [SerializeField] GameObject employeeStatusCard;
    Employee[] employees;

    private void Awake()
    {
        instance = this;
        uiParent.SetActive(false);
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }

        employees = CompanyManager.Instance.GetEmployeeList();
        foreach (Employee employee in employees)
        {
            var card = Instantiate(employeeStatusCard, cardParent);
            card.GetComponent<EmployeeCardUI>().UpdateCardUI(employee);
        }
    }

    public void OpenUI()
    {
        PlayerMovement.Instance.DisableMovement();
        UpdateUI();
        uiParent.SetActive(true);
    }

    public void CloseUI()
    {
        PlayerMovement.Instance.EnableMovement();
        uiParent.SetActive(false);
    }
}
