using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Working : State
{
    Employee employee;
    AStarPathfinder pathfinder;
    List<Vector2Int> path;
    EmployeeJobManager jobManager;
    EmployeeJob currentJob;
    int currentPathIndex;

    public override void OnExit()
    {
    }

    public override void OnStart()
    {
        employee.StartWorking();
        currentPathIndex = -1;
    }

    public override void OnUpdate()
    {
        if (employee.readyForJob)
        {
            StartJob();
        }
        else if (currentPathIndex != -1)
        {
            Vector2Int nextPos = path[currentPathIndex];
            Vector2 employeePos = employee.transform.position;
            if (Vector2.Distance(employeePos, GridManager.WorldTileCenter(nextPos)) > 0.01f)
            {
                float maxMove = employee.moveSpeed * Time.deltaTime;
                employee.transform.position = Vector2.MoveTowards(
                    employeePos,
                    GridManager.WorldTileCenter(nextPos),
                    maxMove
                );
            }
            else if (currentPathIndex < path.Count - 1)
            {
                currentPathIndex++;
            }
            else
            {
                currentPathIndex = -1;
                StartCoroutine(CompleteTask());
            }
        }
    }

    void StartJob()
    {
        employee.readyForJob = false;

        currentJob = jobManager.NewJob();

        Debug.Log("Starting job at " + currentJob.location + " for " + currentJob.duration + " seconds");
        WalkTo(currentJob.location);
    }

    void WalkTo(Vector2Int location)
    {
        Vector2Int cellPosition = GridManager.PositionToCell(employee.transform.position);
        try
        {
            path = pathfinder.FindPath(cellPosition, location);
        }
        catch (InvalidOperationException e) // No path found - add error context
        {
            string msg = "Impossible job location for employee: " + currentJob.location;
            Debug.LogError(msg);
            throw new InvalidOperationException(msg, e);
        }
        currentPathIndex = 0;
    }

    IEnumerator CompleteTask()
    {
        yield return new WaitForSeconds(currentJob.duration);
        employee.readyForJob = true;
    }

    void Awake()
    {
        employee = (Employee)stateMachine;
        pathfinder = new();
        jobManager = new(employee.Type.Role);
    }
}
