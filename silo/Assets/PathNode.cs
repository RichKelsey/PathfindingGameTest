using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathNode : Heap<PathNode>.IHeapItem
{
    public bool IsWalkable;
    public Vector3 WorldPosition;
    public int hCost;
    public int gCost;
    public Vector2Int GridPosition;
    public PathNode Parent;
    int heapIndex;
    
    public PathNode(bool isWalkable, Vector3 worldPosition, Vector2Int gridPosition)
    {
        IsWalkable = isWalkable;
        WorldPosition = worldPosition;
        GridPosition = gridPosition;
    }

    public int FCost()
    {
        return gCost + hCost;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
    
    public int CompareTo(PathNode nodeToCompare)
    {
        int compare = FCost().CompareTo(nodeToCompare.FCost());
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
