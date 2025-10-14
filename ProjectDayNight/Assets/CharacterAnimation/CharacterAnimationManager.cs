using UnityEngine;

/// <summary>
/// Changes animation and sprite flipping based on velocity of object
/// </summary>
public class CharacterAnimationManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;

    private Vector2 posChange;
    private Vector2 lastPosition;
    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        posChange = (Vector2) transform.position - (Vector2) lastPosition;

        if (posChange.magnitude > 0.01f)
        {
            animator.SetBool("isMoving", true);
            if (posChange.x > 0.01)
            {
                sprite.flipX = false;
            }
            else if (posChange.x < 0.01)
            {
                sprite.flipX = true;
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        lastPosition = transform.position;
    }
}
