using UnityEngine;

public class HighlightController : MonoBehaviour
{
    bool isHighlighted = false;

    [SerializeField] Material normalMat;
    [SerializeField] Material highlightMat;

    private SpriteRenderer rend;

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.material = normalMat;
    }


    public void Highlight()
    {
        if (!isHighlighted)
        {
            rend.material = highlightMat;
            isHighlighted = true;
        }
    }
        
    public void DeHighlight()
    {
        if (isHighlighted) 
        {
            rend.material = normalMat;
            isHighlighted = false;
        }

    }
}
