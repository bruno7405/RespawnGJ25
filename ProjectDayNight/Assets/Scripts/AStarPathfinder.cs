using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder
{
    private static readonly float SQRT2 = Mathf.Sqrt(2f);

    private readonly GridData gridData = GridData.Instance;

    private static readonly Vector2Int[] Neighbors =
    {
        new(1, 0),    // E
        new(-1, 0),   // W
        new(0, 1),    // N
        new(0, -1),   // S
        new(1, 1),    // NE
        new(-1, 1),   // NW
        new(1, -1),   // SE
        new(-1, -1),  // SW
    };

    public AStarPathfinder()
    {
        if (gridData == null)
            throw new Exception("GridData not found in Resources. Please create one.");
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        if (!gridData.IsWalkable(start.x, start.y))
            throw new Exception("Start position is not walkable.");
        if (!gridData.IsWalkable(goal.x, goal.y))
            throw new Exception("Goal position is not walkable.");
        if (start == goal)
            return new() { start };

        var openSet = new PriorityQueue<Node>();
        var allNodes = new Dictionary<Vector2Int, Node>();

        Node startNode = GetOrCreateNode(start, allNodes);
        startNode.gCost = 0;
        startNode.hCost = Heuristic(start, goal);
        openSet.Enqueue(startNode);

        while (openSet.Count > 0)
        {
            Node current = openSet.Dequeue();

            if (current.position == goal)
                return ReconstructPath(current);

            current.closed = true;

            foreach (var dir in Neighbors)
            {
                Vector2Int neighborPos = current.position + dir;

                if (!gridData.IsWalkable(neighborPos.x, neighborPos.y)) continue;

                // Prevent cutting corners through obstacles
                if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) == 2)
                {
                    if (!gridData.IsWalkable(current.position.x, current.position.y + dir.y)) continue;
                    if (!gridData.IsWalkable(current.position.x + dir.x, current.position.y)) continue;
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

        // No path found
        return new();
    }

    private static float Heuristic(Vector2Int a, Vector2Int b)
    {
        // Octile distance heuristic for diagonals
        float dx = Mathf.Abs(a.x - b.x);
        float dy = Mathf.Abs(a.y - b.y);
        return dx + dy + (SQRT2 - 2f) * Mathf.Min(dx, dy);
    }

    private static List<Vector2Int> ReconstructPath(Node end)
    {
        var path = new List<Vector2Int>();
        Node current = end;
        while (current != null)
        {
            path.Add(current.position);
            current = current.parent;
        }
        path.Reverse();
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