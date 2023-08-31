using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class NodeGrid : MonoBehaviour
{
    public Tilemap obstacleMap;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public bool drawGizmos;
    
    private PathNode[,] _grid;

    private float _nodeDiameter;
    private int _gridSizeX, _gridSizeY;
    private bool _obstacleNearby;
    

    void Awake()
    {
        _nodeDiameter = nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }
    
    public int MaxSize => _gridSizeX * _gridSizeY;

    public PathNode NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x/ 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y/ 2) / gridWorldSize.y;
        
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return _grid[x, y];

    }

    public List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();
        _obstacleNearby = false;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                
                int checkX = node.GridPosition.x + x;
                int checkY = node.GridPosition.y + y;
                
                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    neighbours.Add(_grid[checkX, checkY]);
                }

                if (_grid[checkX, checkY].IsWalkable == false)
                {
                    _obstacleNearby = true;
                }
            }
        }

        if (_obstacleNearby)
        {
            //a janky solution to disable diagonal movement if there is an obstacle nearby
            //removes the diagonal (NE,NW,SE,SW) neighbours from the list
            neighbours.RemoveAt(7);neighbours.RemoveAt(5);neighbours.RemoveAt(2);neighbours.RemoveAt(0);
        }
        
        return neighbours;
    }

    void CreateGrid()
    {
        _grid = new PathNode[_gridSizeX, _gridSizeY];
        Vector2 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * _nodeDiameter + nodeRadius) + Vector2.up * (y * _nodeDiameter + nodeRadius);
                _grid[x, y] = new PathNode(false, worldPoint, new Vector2Int(x,y));

                if (obstacleMap.HasTile(obstacleMap.WorldToCell(_grid[x, y].WorldPosition)))
                    _grid[x, y].IsWalkable = false;
                else
                    _grid[x, y].IsWalkable = true;


            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
 
        if (_grid != null && drawGizmos)
        {
            foreach (PathNode node in _grid)
            {
                Gizmos.color= node.IsWalkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.WorldPosition, Vector2.one * (_nodeDiameter - .1f));
            }
        }
    }
}
