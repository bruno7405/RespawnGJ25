using UnityEngine;

/// <summary>
/// Changes animation and sprite flipping based on velocity of object
/// </summary>
public class CharacterAnimationManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] ParticleSystem walkParticles;

    void Start()
    {
        var emission = walkParticles.emission;
        emission.enabled = false;
    }

    public void ToggleWalk()
    {
        animator.SetBool("isMoving", true);
        var emission = walkParticles.emission;
        emission.enabled = true;
    }

    public void ToggleIdle()
    {
        animator.SetBool("isMoving", false);
        var emission = walkParticles.emission;
        emission.enabled = false;
    }

    public void FaceLeft()
    {
        sprite.flipX = true;
    }

    public void FaceRight()
    {
        sprite.flipX = false;
    }
    public void StopAnimation()
    {
        animator.enabled = false;
        var emission = walkParticles.emission;
        emission.enabled = false;
    }
    public void StartAnimation()
    {
        animator.enabled = true;
        var emission = walkParticles.emission;
        emission.enabled = true;
    }
    void OnDisable()
    {
        StopAnimation();
    }

}
