using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Dictionary<Vector2, GameObject> _tiles;
    private float originOffset = 0f;

    public GridManager(int width, int height, GameObject TilePrefab)
    { 
        _tiles = new Dictionary<Vector2, GameObject>();

        for (int x = 0; x < GameData.Width; x++)
        {
            for (int y = 0; y < GameData.Height; y++)
            {
                //Position of the tile - taking care of grid offset
                Vector3 pos = new Vector3(x + originOffset, y + originOffset);
                 // Instantiating a new tile
                GameObject tile = Instantiate(TilePrefab, pos , Quaternion.identity);
                //Set is an obstacle or not
                tile.GetComponent<Tile>().setObstacle(false);
                //Storing the tile in the dictionary
                _tiles[new Vector2(pos.x, pos.y)] = tile;
            }
        }
    }

    public bool isTileAvailable(Vector3 pos)
    {
        //Converting a vector3 to vector2Int
        Vector2 tilePos = new Vector2(pos.x, pos.y);
        //Check if the tile exists in our dictionary
        if (_tiles.ContainsKey(tilePos))
        {
            //If it exists, we check if it is an obstacle
            if (_tiles[tilePos].GetComponent<Tile>().isObstacle())
            {
                //If it is an obstacle, we return false
                return false;
            }
            else
            {
                //If it is not an obstacle, we return true
                return true;
            }
        }
        else
        {
            //Does not exist
            return false;
        }
    }


    public Vector3 getSpawnLocation()
    {
        while (true)
        {
            Vector3 randomPos = new Vector3(Random.Range(0, GameData.Width-2), Random.Range(0, GameData.Height-2));
            if (isTileAvailable(randomPos))
            {
                return randomPos;
            }
        }
    }

    public Vector3 getSpawnLocation(GameObject player)
    {
        while (true)
        {
            Vector3 randomPos = new Vector3(Random.Range(0, GameData.Width - 2), Random.Range(0, GameData.Height - 2));
            if (isTileAvailable(randomPos) && Vector3.Distance(randomPos, player.transform.position) > 10)
            {
                return randomPos;
            }
        }
    }
}
