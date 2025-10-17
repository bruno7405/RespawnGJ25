using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder
{
    private static readonly float SQRT2 = Mathf.Sqrt(2f);

    private static readonly Vector2Int[] Neighbors =
    {
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.down,
        new(1, 1),    // NE
        new(-1, 1),   // NW
        new(1, -1),   // SE
        new(-1, -1),  // SW
    };

    public AStarPathfinder()
    {
        if (GridManager.Grid == null)
            throw new Exception("GridManager not initialized. Please load grid.");
    }

    public List<Vector2> FindPath(Vector2Int start, Vector2Int goal, Vector2 preciseGoal = default)
    {
        // if (!GridManager.IsWalkable(start.x, start.y))
        //     throw new ArgumentException($"Start position is not walkable. {GridManager.WorldTileCenter(start)}");
        // if (!GridManager.IsWalkable(goal.x, goal.y))
        //     throw new ArgumentException($"Goal position is out of bounds. {GridManager.WorldTileCenter(goal)}");
        if (!GridManager.IsInBounds(start))
            throw new ArgumentException($"Start position is out of bounds. {GridManager.WorldTileCenter(start)}");
        if (!GridManager.IsInBounds(goal))
            throw new ArgumentException($"Goal position is out of bounds. {GridManager.WorldTileCenter(goal)}");
        if (preciseGoal != default && !GridManager.IsInBounds(GridManager.PositionToCell(preciseGoal)))
            throw new ArgumentException($"Goal position is out of bounds. {preciseGoal}");
        
        if (start == goal) return new() { start };

        var openSet = new PriorityQueue<Node>();
        var allNodes = new Dictionary<Vector2Int, Node>();

        Node startNode = GetOrCreateNode(start, allNodes);
        startNode.gCost = 0;
        startNode.hCost = Heuristic(start, goal);
        openSet.Enqueue(startNode);

        while (openSet.Count > 0)
        {
            Node current = openSet.Dequeue();

            if (current.position == goal) return ReconstructPath(current, preciseGoal);

            current.closed = true;

            foreach (var dir in Neighbors)
            {
                Vector2Int neighborPos = current.position + dir;

                if (neighborPos != goal && !GridManager.IsWalkable(new(neighborPos.x, neighborPos.y))) continue;

                // Prevent cutting corners through obstacles
                if (neighborPos != goal && dir.magnitude == SQRT2)
                {
                    if (!GridManager.IsWalkable(new(current.position.x, current.position.y + dir.y))) continue;
                    if (!GridManager.IsWalkable(new(current.position.x + dir.x, current.position.y))) continue;
                }

                Node neighbor = GetOrCreateNode(neighborPos, allNodes);
                if (neighbor.closed)
                    continue;

                float moveCost = (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) == 2) ? 1.4142f : 1f; // √2 for diagonals
                float tentativeG = current.gCost + moveCost;

                if (tentativeG < neighbor.gCost)
                {
                    neighbor.parent = current;
                    neighbor.gCost = tentativeG;
                    neighbor.hCost = Heuristic(neighbor.position, goal);
                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor);
                }
            }
        }

        throw new InvalidOperationException("No path exists between the specified locations");
    }

    private static float Heuristic(Vector2Int a, Vector2Int b)
    {
        // Octile distance heuristic for diagonals
        float dx = Mathf.Abs(a.x - b.x);
        float dy = Mathf.Abs(a.y - b.y);
        return dx + dy + (SQRT2 - 2f) * Mathf.Min(dx, dy);
    }

    private static List<Vector2> ReconstructPath(Node end, Vector2 preciseGoal)
    {
        List<Vector2> path = new();
        Node current = end;
        while (current != null)
        {
            path.Add(GridManager.WorldTileCenter(current.position));
            current = current.parent;
        }
        path.Reverse();
        if (preciseGoal != default) path[^1] = preciseGoal;
        // Debug.Log(string.Join(" -> ", path));
        return path;
    }

    private static Node GetOrCreateNode(Vector2Int pos, Dictionary<Vector2Int, Node> all)
    {
        if (!all.TryGetValue(pos, out Node node))
        {
            node = new Node(pos);
            all[pos] = node;
        }
        return node;
    }

    // Node + simple priority queue
    private class Node
    {
        public Vector2Int position;
        public float gCost = float.MaxValue;
        public float hCost;
        public float fCost => gCost + hCost;
        public Node parent;
        public bool closed;

        public Node(Vector2Int pos) => position = pos;
    }

    private class PriorityQueue<T> where T : Node
    {
        private readonly List<T> items = new List<T>();
        public int Count => items.Count;

        public void Enqueue(T item)
        {
            items.Add(item);
            items.Sort((a, b) => a.fCost.CompareTo(b.fCost));
        }

        public T Dequeue()
        {
            var first = items[0];
            items.RemoveAt(0);
            return first;
        }

        public bool Contains(T item) => items.Contains(item);
    }
}