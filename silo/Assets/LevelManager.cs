using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    
    public  Tilemap obstacleMap;
    
    private TileBase[] _obstacleTiles;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var position in obstacleMap.cellBounds.allPositionsWithin)
        {
            var tile = obstacleMap.GetTile(position);
            Debug.Log(tile != null ? tile.name : "null tile");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void CreateSpawnpoints()
    {
        
    }
}
