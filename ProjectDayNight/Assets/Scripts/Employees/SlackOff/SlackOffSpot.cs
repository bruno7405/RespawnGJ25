using System;
using UnityEngine;

public class SlackOffSpot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Vector2 locationOffset;
    [SerializeField] private SlackOffRoom slackOffRoom;
    public bool IsAssigned { get; private set; }
    public SlackOffRoom Room => slackOffRoom;
    private Vector2 location;
    public Vector2 Location => location;

    public void Assign()
    {
        IsAssigned = true;
    }
    public void Unassign()
    {
        IsAssigned = false;
    }

    void Awake()
    {
        SlackOffSpots.Register(this);
        IsAssigned = false;
        location = locationOffset + (sr == null ? (Vector2)transform.position : new(transform.position.x, sr.bounds.min.y));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(locationOffset + new Vector2(transform.position.x, sr.bounds.min.y), 0.05f);
    }
}