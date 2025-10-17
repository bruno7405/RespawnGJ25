using UnityEngine;

public class StatusIconBruno : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HighMorale()
    {
        animator.SetTrigger("HighMorale");
    }

    public void MediumMorale()
    {
        animator.SetTrigger("MediumMorale");
    }

    public void LowMorale()
    {
        animator.SetTrigger("LowMorale");
    }

    public void Slacking()
    {
        animator.SetTrigger("Slacking");
    }

    public void Sleeping()
    {
        animator.SetTrigger("Sleeping");
    }

    public void Default()
    {
        animator.SetTrigger("Default");
    }
}
