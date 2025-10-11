using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static bool canAttack = true;

    [SerializeField] private float attackRange = 2f;
    [SerializeField] LayerMask employeeLayer;
    Transform nearestEmployee;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Attack()
    {
        if (!canAttack) return;
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange, employeeLayer);
        if (hitColliders.Length == 0) return;

        float nearestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEmployee = hitCollider.transform;
            }
        }

        if (nearestEmployee != null)
        {
            Vector2 direction = nearestEmployee.position - transform.position;
            transform.position += new Vector3(direction.x * 2, direction.y * 2, 0);

        }


    }
}
