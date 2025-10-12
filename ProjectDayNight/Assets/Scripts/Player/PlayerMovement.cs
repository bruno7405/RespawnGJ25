using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    Vector2 movementVector;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementVector * speed * Time.fixedDeltaTime;
    }

    public void HandleMovement(Vector2 v)
    {
        v.Normalize();
        movementVector = v;
    }
}
