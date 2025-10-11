using Unity.VisualScripting;
using UnityEngine;

public class Employee : StateMachineManager
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] string employeeName = "Ben";
    [SerializeField] int wage = 10;
    [SerializeField] int revenue = 30;
    [SerializeField] int morale = 80;

    public int Morale
    {
        get => morale;
        private set
        {
            morale = value;
            CompanyManager.Instance?.RecalculateMorale();
        }
    }
    public bool working { get; private set; } = true;
    public const int MAX_MORALE = 100;
    void Awake()
    {

    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
