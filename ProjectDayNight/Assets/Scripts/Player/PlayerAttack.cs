using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerAttack : MonoBehaviour
{
    private static PlayerAttack instance;
    public static PlayerAttack Instance => instance;

    public static bool canAttack = true;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] LayerMask employeeLayer;
    [SerializeField] LayerMask wallLayer;
    Transform nearestEmployee;

    [SerializeField] Texture2D normalCursor;
    [SerializeField] Texture2D attackCursor;


    Rigidbody2D rb;
    Vector2 raycastHitPoint = new Vector2(100, 100);

    GameStateManager gameManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        gameManager = GameStateManager.Instance;
    }

    public void Attack()
    {
        if (!canAttack) return;

        nearestEmployee = null;
        float nearestDistance = Mathf.Infinity;

        // Find nearest employee within attack range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange, employeeLayer);
        if (hitColliders.Length == 0) return;

        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
            if (Physics2D.Raycast(transform.position, hitCollider.transform.position - transform.position, distance, wallLayer)) 
            {
                continue;
            }

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEmployee = hitCollider.transform;
            }
        }

        // Kill the nearest employee
        if (nearestEmployee == null) return;
        var employee = nearestEmployee.GetComponent<Employee>();
        if (employee.StateName is EmployeeState.Dead
            or EmployeeState.Working
            or EmployeeState.Sleeping)
            return;
        // Move player
        Vector2 direction = (nearestEmployee.position - transform.position) * 0.9f;
        transform.position += (Vector3)direction;

        // Kill employee
        // TODO VERY UNSEMANTIC, THIS IS ALL IN ATTACK METHOD/FILE
        if (!gameManager.IsDay) employee.KillEmployee();
        else employee.UrgeWork();

    }

    public void IncreaseRange(int percentage)
    {
        attackRange += attackRange * (percentage / 100f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(raycastHitPoint, 0.05f);
    }
}
