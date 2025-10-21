using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private static PlayerAttack instance;
    public static PlayerAttack Instance => instance;

    public static bool canAttack = true;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] LayerMask employeeLayer;
    [SerializeField] LayerMask wallLayer;

    [SerializeField] Texture2D normalCursor;
    [SerializeField] Texture2D attackCursor;

    Collider2D[] hitColliders;
    Transform nearestEmployee;
    float nearestDistance;
    Vector2 raycastHitPoint = new Vector2(100, 100);

    GameStateManager gameManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;
    }

    private void Start()
    {
        gameManager = GameStateManager.Instance;
    }

    private void Update()
    {
        // Find nearest employee within attack range
        hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange, employeeLayer);
        if (hitColliders.Length == 0)
        {
            nearestEmployee = null;
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
            return;
        }

        Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);

        nearestDistance = Mathf.Infinity;
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

    }

    public void Attack()
    {
        if (!canAttack) return;

        // Kill the nearest employee
        if (nearestEmployee == null) return;
        var employee = nearestEmployee.GetComponent<Employee>();
        // Move player
        //Vector2 direction = (nearestEmployee.position - transform.position) * 0.5f;
        //transform.position += (Vector3)direction;

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

        if (nearestEmployee) Gizmos.DrawLine(transform.position, nearestEmployee.position);
    }
}
