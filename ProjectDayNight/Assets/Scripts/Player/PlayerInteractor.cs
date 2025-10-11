using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] Transform nearestInteractable = null;

    private void Update()
    {
        SelectNearestInteractable();
    }

    private void SelectNearestInteractable()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactableLayer);
        if (hitColliders.Length == 0) return;

        float nearestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestInteractable = hitCollider.transform;
            }
        }
    }

    public void Interact()
    {
        nearestInteractable?.GetComponent<IInteractable>()?.OnInteract(this);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        if (nearestInteractable == null) return;
        Gizmos.color = Color.beige;
        Gizmos.DrawLine(transform.position, nearestInteractable.position);
    }
}
