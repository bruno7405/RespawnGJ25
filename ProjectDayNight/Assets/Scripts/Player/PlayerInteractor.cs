using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private static PlayerInteractor instance;
    public static PlayerInteractor Instance => instance;

    [SerializeField] private float interactionRange = 2f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] Transform nearestInteractable = null;
    [SerializeField] CircleCollider2D interactionCollider;

    [SerializeField] GameObject informationUI; // pops up when near an interactable

    private List<Transform> interactablesInRange = new List<Transform>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;

        interactionCollider.radius = interactionRange;
        informationUI.SetActive(false);
    }

    private void Update()
    {
        SelectNearestInteractable();
    }

    private void SelectNearestInteractable()
    {
        if (interactablesInRange.Count == 0)
        {
            nearestInteractable = null;
            informationUI.SetActive(false);
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
        informationUI.SetActive(true);
        var pos = nearestInteractable.position;
        informationUI.transform.position = new Vector2(pos.x, nearestInteractable.GetComponent<SpriteRenderer>().bounds.min.y - 0.15f);
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

    public void IncreaseRange(int percentage)
    {
        interactionRange += interactionRange * (percentage / 100f);
        interactionCollider.radius = interactionRange;
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
