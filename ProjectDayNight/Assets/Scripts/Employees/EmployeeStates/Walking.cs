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
        pTransform = parent.transform;
        currentPathIndex = 0;
        pathfinder = new();
        Vector2Int cellPosition = GridManager.PositionToCell(pTransform.position);
        path = pathfinder.FindPath(cellPosition, new(0, 0));
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
            // else exit state
        }
    }

    public override void OnExit()
    {
    }
}