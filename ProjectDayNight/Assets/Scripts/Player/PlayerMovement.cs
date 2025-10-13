using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;
    public static PlayerMovement Instance => instance;

    [SerializeField] float speed;
    bool canMove = true;

    Vector2 movementVector;
    Rigidbody2D rb;

    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        rb.linearVelocity = movementVector * speed * Time.fixedDeltaTime;
    }

    public void HandleMovement(Vector2 v)
    {
        v.Normalize();
        movementVector = v;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
    }
}
