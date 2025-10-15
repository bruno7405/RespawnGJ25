using System;
using UnityEngine;

public class EmployeeJob : MonoBehaviour
{
    [SerializeField] private string jobName;
    [SerializeField] private string description;
    [Range(1, 10)]
    [SerializeField] private int duration;
    [SerializeField] private Role[] allowedRoles = (Role[])Enum.GetValues(typeof(Role));
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Vector2 locationOffset;
    public int Duration => duration;
    public Role[] AllowedRoles => allowedRoles;
    public string Name => jobName;
    public string Description => description;
    public bool IsAssigned { get; private set; }
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
        EmployeeJobRegistry.RegisterJob(this);
        IsAssigned = false;
        location = locationOffset + (sr == null ? (Vector2)transform.position : new(transform.position.x, sr.bounds.min.y));
    }
}