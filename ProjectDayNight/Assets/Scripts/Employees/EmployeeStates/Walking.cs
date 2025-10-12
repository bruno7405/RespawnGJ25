using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Walking : State
{
    [SerializeField] private float moveSpeed;
    private AStarPathfinder pathfinder;
    private List<Vector2Int> path;
    private int currentPathIndex;
    private Transform pTransform;

    public override void OnStart()
    {
        Debug.Log("Started Walking");
        pTransform = parent.transform;
        currentPathIndex = 0;
        pathfinder = new();
        Vector2Int cellPosition = GridManager.PositionToCell(pTransform.position);
        path = pathfinder.FindPath(cellPosition, new(0, 0));
        foreach (Vector2Int pos in path)
        {
            Debug.Log(pos);
        }
    }

    public override void OnUpdate()
    {
        Vector2Int nextPos = path[currentPathIndex];
        if (Vector2.Distance(pTransform.position, GridManager.WorldTileCenter(nextPos)) > 0.01f)
        {
            float maxMove = moveSpeed * Time.deltaTime;
            pTransform.position = Vector2.MoveTowards(
                pTransform.position,
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
            Debug.Log("Reached destination!");
            // else exit state
        }
    }

    public override void OnExit()
    {
        Debug.Log("Stopped Walking");
    }
}