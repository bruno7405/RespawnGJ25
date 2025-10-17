using UnityEngine;

public class BlackScreenUI : MonoBehaviour
{

    [SerializeField] GameObject root;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        root.SetActive(true);
        animator.SetTrigger("fadeIn");
    }

    public void FadeOut()
    {
        root.SetActive(true);
        animator.SetTrigger("fadeOut");
    }

    public void Black()
    {
        root.SetActive(true);
        animator.SetTrigger("black");
    }
}
