using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;
    public static PlayerMovement Instance => instance;

    [SerializeField] float speed;
    [SerializeField] CharacterAnimationManager charVisuals;

    bool canMove = true;
    Vector2 movementVector;
    Rigidbody2D rb;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;

        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        MinimapManager.Instance.RegisterBoss();
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
        
        // Change visuals
        if (canMove)
        {
            if (v == Vector2.zero)
            {
                charVisuals.ToggleIdle();
            }
            else
            {
                charVisuals.ToggleWalk();
                if (v.x > 0) charVisuals.FaceRight();
                else if (v.x < 0) charVisuals.FaceLeft();

            }
        }
        else
        {
            charVisuals.ToggleIdle();
        }
        
    }

    public void IncreaseSpeed(int percentage)
    {
        speed *= (1 + (percentage / 100f));
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
