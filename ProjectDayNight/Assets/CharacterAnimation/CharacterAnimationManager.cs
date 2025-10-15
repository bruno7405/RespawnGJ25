using UnityEngine;

/// <summary>
/// Changes animation and sprite flipping based on velocity of object
/// </summary>
public class CharacterAnimationManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] ParticleSystem walkParticles;
    

    private Vector2 posChange;
    private Vector2 lastPosition;

    void Start()
    {
        lastPosition = transform.position;

        var emission = walkParticles.emission;
        emission.enabled = false;
    }

    void Update()
    {
        posChange = (Vector2) transform.position - (Vector2) lastPosition;

        if (posChange.magnitude > 0.01)
        {
            animator.SetBool("isMoving", true);
            if (posChange.x > 0.005f)
            {
                sprite.flipX = false;
            }
            else if (posChange.x < 0.005f)
            {
                sprite.flipX = true;
            }

            // Particles
            var emission = walkParticles.emission;
            emission.enabled = true;
        }
        else
        {
            animator.SetBool("isMoving", false);
            var emission = walkParticles.emission;
            emission.enabled = false;
        }

        lastPosition = transform.position;
    }
}
