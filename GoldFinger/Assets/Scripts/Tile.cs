using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool _isObstacle = false;

    public bool isVisited = false;

    public bool isObstacle()
    {
        return _isObstacle;
    }

    public void setObstacle(bool isObstacle)
    {
        //this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        _isObstacle = isObstacle;
        this.gameObject.transform.GetComponent<SpriteRenderer>().enabled = !isObstacle;
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = isObstacle;
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = isObstacle;
    }
}
