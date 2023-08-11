using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;

public class Pathfinding : MonoBehaviour
{
    private NodeGrid _grid;
    private Heap<PathNode> _openSet;
    private HashSet<PathNode> _closedSet;
    private PathRequestManager _requestManager;
    
    void Awake()
    {
        _grid = GetComponent<NodeGrid>();
        _requestManager = GetComponent<PathRequestManager>();
    }
    
    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        StartCoroutine(FindPath(startPosition, targetPosition));
    }
    
    private IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        if (_openSet.IsUnityNull())
        {
            Debug.Log(_grid);
            _openSet = new Heap<PathNode>(_grid.MaxSize);
        }

        if (_closedSet.IsUnityNull())
        {
            _closedSet = new HashSet<PathNode>();
        }
        
        
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        
        PathNode startNode = _grid.NodeFromWorldPoint(startPosition);
        PathNode targetNode = _grid.NodeFromWorldPoint(targetPosition);

        if (startNode.IsWalkable && targetNode.IsWalkable)
        {
            _openSet.Clear();
            _closedSet.Clear();
            _openSet.Add(startNode);

            while (_openSet.Count > 0)
            {
                PathNode currentNode = _openSet.RemoveFirst();

                _closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (PathNode neighbour in _grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.IsWalkable || _closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !_openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;
                        if (!_openSet.Contains(neighbour))
                        {
                            _openSet.Add(neighbour);
                        }
                    }
                }
            }
        }

        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        _requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }
    
    private Vector3[] RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<PathNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].GridPosition.x - path[i].GridPosition.x, path[i - 1].GridPosition.y - path[i].GridPosition.y);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].WorldPosition);
            }

            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }
    
    private int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridPosition.x - nodeB.GridPosition.x);
        int dstY = Mathf.Abs(nodeA.GridPosition.y - nodeB.GridPosition.y);
        
        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
