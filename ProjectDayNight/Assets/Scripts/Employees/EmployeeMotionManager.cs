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
    /// <returns>Action to force cancel path</returns>
    public Action WalkTo(Vector2 destination, Action callback = null)
    {
        currentSpeed = walkSpeed;
        SetPath(destination, callback);
        return () => { currentPathIndex = -1; currentPath = null; currentCallback = null; };
    }
    /// <summary>
    /// Makes employee run to location
    /// </summary>
    /// <param name="destination">Tilemap Coordinates</param>
    /// <param name="callback">Callback to invoke after reaching destination</param>
    /// <returns>Action to force cancel path</returns>
    public Action RunTo(Vector2 destination, Action callback = null)
    {
        currentSpeed = runSpeed;
        SetPath(destination, callback);
        return () => { currentPathIndex = -1; currentPath = null; currentCallback = null; };
    }

    void SetPath(Vector2 location, Action callback = null)
    {
        currentCallback = callback;
        Vector2Int currentCell = GridManager.PositionToCell(transform.position);
        Vector2Int goalCell = GridManager.PositionToCell(location);
        try
        {
            currentPath = pathfinder.FindPath(currentCell, goalCell);
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

    private void Start()
    {
        pathfinder = new();
        currentSpeed = walkSpeed;
    }

    void Awake()
    {
        currentPath = null;
        currentPathIndex = -1;
    }
}