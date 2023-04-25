using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс, который будет отвечать за поиск пути по нашему графу.
/// В редакторе есть возможность задать стартовую вершину и конечную (куда мы хотим найти путь).
/// 
/// Так же он визуально подкрашивает все вершины, которые он анализировал при
/// построении пути.
/// 
/// Другим цветом отмечает итоговый найденный путь.
/// </summary>
public class PathfindingEngine : MonoBehaviour
{
    public List<Node> GetPath(Node start, Node destination)
    {
        var parentNode = new Dictionary<Node, Node>();
        var visited = new HashSet<Node>();
        
        var nextNodes = new Queue<Node>();
        var current = start;
        
        EnqueueAllNeighbours(current, visited, parentNode, nextNodes);

        var isPathFound = false;
        
        while (nextNodes.Count > 0)
        {
            current = nextNodes.Dequeue();

            if (current == destination)
            {
                isPathFound = true;
                break;
            }

            // skip already visited or not walkable nodes
            if (visited.Contains(current) || !current.IsWalkable)
            {
                continue;
            }
            
            EnqueueAllNeighbours(current, visited, parentNode, nextNodes);
            
            visited.Add(current);
        }

        if (!isPathFound)
        {
            return null;
        }

        var result = new List<Node> { current };
        while (current != start)
        {
            current = parentNode[current];
            result.Add(current);
        }

        result.Reverse();
        
        return result;
    }

    private static void EnqueueAllNeighbours(Node current, HashSet<Node> visited, Dictionary<Node, Node> routes, Queue<Node> nextNodes)
    {
        foreach (var neighbour in current.GetNeighbours())
        {
            if (visited.Contains(neighbour) || !neighbour.IsWalkable)
            {
                continue;
            }
            
            routes[neighbour] = current;
            nextNodes.Enqueue(neighbour);
        }
    }

    public List<Node> GetAllReachableNodes(Node start)
    {
        var result = new HashSet<Node>();

        var nextNodes = new Queue<Node>(start.GetNeighbours());
        while(nextNodes.Count > 0)
        {
            var nextNode = nextNodes.Dequeue();
            
            // skip already visited or not walkable nodes
            if (result.Contains(nextNode) || !nextNode.IsWalkable)
            {
                continue;
            }
            
            foreach (var neighbour in nextNode.GetNeighbours())
            {
                nextNodes.Enqueue(neighbour);
            }
            
            result.Add(nextNode);
        }

        return result.ToList();
    }
}