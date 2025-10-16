using System;
using UnityEngine;

public class SlackOffSpot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Vector2 locationOffset;
    [SerializeField] private SlackOffSpotType slackOffSpotType;
    public bool IsAssigned { get; private set; }
    public SlackOffSpotType Type => slackOffSpotType;
    private Vector2 location;
    public Vector2 Location => location;

    public void Assign()
    {
        if (IsAssigned) throw new InvalidOperationException("Job is already assigned");
        IsAssigned = true;
        // Visuals
        sr.color = Color.cyan;
    }
    public void Unassign()
    {
        IsAssigned = false;
        // Reset color
        sr.color = Color.white;
    }

    void Awake()
    {
        SlackOffSpots.Register(this);
        IsAssigned = false;
        location = locationOffset + (sr == null ? (Vector2)transform.position : new(transform.position.x, sr.bounds.min.y));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(locationOffset + new Vector2(transform.position.x, sr.bounds.min.y), 0.05f);
    }
}