using Unity.VisualScripting;
using UnityEngine;

public class BossTerminal : MonoBehaviour, IInteractable
{
    [SerializeField] EmployeeStatusUI employeeStatusUI;

    public void OnInteract(PlayerInteractor interactor)
    {
        employeeStatusUI.OpenUI();
    }
}
