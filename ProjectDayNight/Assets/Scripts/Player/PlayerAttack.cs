using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerAttack : MonoBehaviour
{
    public static bool canAttack = true;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] LayerMask employeeLayer;
    [SerializeField] LayerMask wallLayer;
    Transform nearestEmployee;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
                Debug.Log("Wall in the way");
                continue;
            }

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEmployee = hitCollider.transform;
            }
        }

        // Kill the nearest employee
        if (nearestEmployee != null)
        {
            // Move player
            Vector2 direction = nearestEmployee.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, nearestDistance * 2, wallLayer);
            if (hit) // If there's a wall in the way, move towards the wall instead
            {
                Vector2 d = (hit.point - (Vector2) transform.position) * 0.8f;
                transform.position += new Vector3(d.x, d.y, 0);
            }
            else // Teleport twice the distance towards the employee
            {
                transform.position += new Vector3(direction.x * 2, direction.y * 2, 0);
            }

            // Kill employee
            nearestEmployee.GetComponent<Employee>().KillEmployee();
        }


    }
}
