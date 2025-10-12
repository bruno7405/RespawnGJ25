using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeMotionManager : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    public float WalkSpeed => walkSpeed;
    [SerializeField] private float runSpeed;
    public float RunSpeed => runSpeed;
    private AStarPathfinder pathfinder;
    private List<Vector2Int> currentPath;
    private int currentPathIndex;
    private float currentSpeed;
    private Action currentCallback;

    /// <summary>
    /// Makes employee walk to location
    /// </summary>
    /// <param name="destination">Tilemap Coordinates</param>
    /// <param name="callback">Callback to invoke after reaching destination</param>
    public void WalkTo(Vector2Int destination, Action callback = null)
    {
        currentSpeed = walkSpeed;
        SetPath(destination, callback);
    }
    /// <summary>
    /// Makes employee run to location
    /// </summary>
    /// <param name="destination">Tilemap Coordinates</param>
    /// <param name="callback">Callback to invoke after reaching destination</param>
    public void RunTo(Vector2Int destination, Action callback = null)
    {
        currentSpeed = runSpeed;
        SetPath(destination, callback);
    }

    void SetPath(Vector2Int location, Action callback = null)
    {
        currentCallback = callback;
        Vector2Int cellPosition = GridManager.PositionToCell(transform.position);
        try
        {
            currentPath = pathfinder.FindPath(cellPosition, location);
        }
        catch (InvalidOperationException e) // No path found - add error context
        {
            string msg = "Impossible path for employee: " + location;
            Debug.LogError(msg);
            throw new InvalidOperationException(msg, e);
        }
        currentPathIndex = 0;
    }

    void Update()
    {
        if (currentPath == null || currentPathIndex == -1) return;

        Vector2Int nextPos = currentPath[currentPathIndex];
        Vector2 employeePos = transform.position;
        if (Vector2.Distance(employeePos, GridManager.WorldTileCenter(nextPos)) > 0.01f)
        {
            float maxMove = currentSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(
                employeePos,
                GridManager.WorldTileCenter(nextPos),
                maxMove
            );
        }
        else if (currentPathIndex < currentPath.Count - 1)
        {
            currentPathIndex++;
        }
        else // Path complete
        {
            currentPathIndex = -1;
            currentPath = null;
            currentCallback?.Invoke();
            currentCallback = null;
        }
    }

    void Awake()
    {
        currentPath = null;
        currentPathIndex = -1;
        pathfinder = new();
        currentSpeed = walkSpeed;
    }
}