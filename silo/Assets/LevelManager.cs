using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = Unity.Mathematics.Random;

public class LevelManager : MonoBehaviour
{
    public TileBase spawnpointTile;
    public  Tilemap obstacleMap;
    
    private List<Vector3Int> _obstacleTiles;
    
    public int numOfSpawnpoints;
    
    // Start is called before the first frame update
    void Start()
    {
        _obstacleTiles = new List<Vector3Int>();
        int i = 0;
        foreach (var position in obstacleMap.cellBounds.allPositionsWithin)
        {
            var tile = obstacleMap.GetTile(position);
            if (tile != null)
            {
                _obstacleTiles.Add(position);
                i++;
            } 
        }
        
        CreateSpawnpoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void CreateSpawnpoints()
    {
        for (int i = 0; i < numOfSpawnpoints; i++)
        {
            obstacleMap.SetTile(_obstacleTiles[UnityEngine.Random.Range(0, _obstacleTiles.Count + 1)], spawnpointTile);
        }
    }
}
