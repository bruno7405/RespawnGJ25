using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] Transform nearestInteractable = null;

    private List<Transform> interactablesInRange = new List<Transform>();
    private float nearestDistance = Mathf.Infinity;

    private void Update()
    {
        SelectNearestInteractable();
    }

    private void SelectNearestInteractable()
    {
        if (interactablesInRange.Count == 0)
        {
            nearestInteractable = null;
            return;
        }

        float nearestDistance = Mathf.Infinity;

        foreach (var interactable in interactablesInRange)
        {
            float distance = Vector2.Distance(transform.position, interactable.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestInteractable = interactable.transform;
            }
        }

        nearestInteractable?.GetComponent<HighlightController>()?.Highlight();
    }

    public void Interact()
    {
        nearestInteractable?.GetComponent<IInteractable>()?.OnInteract(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 6) return;
        interactablesInRange.Add(collision.transform);
        SelectNearestInteractable();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 6) return;
        collision.gameObject.GetComponent<HighlightController>()?.DeHighlight();
        interactablesInRange.Remove(collision.transform);
        SelectNearestInteractable();
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
