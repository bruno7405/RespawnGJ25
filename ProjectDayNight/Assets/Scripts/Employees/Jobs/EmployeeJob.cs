using System;
using UnityEngine;

public class EmployeeJob : MonoBehaviour
{
    [SerializeField] private string jobName;
    [SerializeField] private string description;
    [Range(0f, 10f)]
    [SerializeField] private float duration;
    [SerializeField] private Role[] allowedRoles = (Role[])Enum.GetValues(typeof(Role));
    [SerializeField] private SpriteRenderer sr;
    public float Duration => duration;
    public Role[] AllowedRoles => allowedRoles;
    public string Name => jobName;
    public string Description => description;
    public bool IsAssigned { get; private set; }
    public Vector2 Location => sr == null ? transform.position : new(transform.position.x, sr.bounds.min.y);

    public void Assign()
    {
        if (IsAssigned) throw new System.InvalidOperationException("Job is already assigned");
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
    }
}