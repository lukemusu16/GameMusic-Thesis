using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;

public class mySeeker : MonoBehaviour
{
    private IAstarAI[] _ais;

    [SerializeField]
    private GameObject _waypoint;

    private GameManager gm;

    private List<GameObject> wps = new List<GameObject>(10);

    public bool hasStarted = false;

    public bool isFleeing = false;

    private Transform fleewaypoint;


    GameObject player;

    float currentSpeed;


    private GameDiff currentDiff;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _ais = FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray();

        player = GameObject.FindGameObjectWithTag("Player");

        currentDiff = GameData.CurrentDiff;

        switch (currentDiff)
        {
            case (GameDiff.Easy):
                GetComponent<AILerp>().speed = 2;
                break;

            case (GameDiff.Medium):
                GetComponent<AILerp>().speed = 3;
                break;

            case (GameDiff.Hard):
                GetComponent<AILerp>().speed = 4;
                break;
        }

        currentSpeed = GetComponent<AILerp>().speed;

    }

    void Update()
    {
        if (hasStarted)
        {

            if (!isFleeing)
            {
                this.gameObject.GetComponent<AIDestinationSetter>().target = player.transform;
                //_ais[100].SearchPath();
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.blue, Color.white, Mathf.PingPong(Time.time, 0.6f));
            }
            
            
        }
        
    }

    public Transform FleeWaypoint()
    {
        Vector3 waypoint;

        //waypoint = new Vector3(Random.Range(-(GameData.Width / 2), (GameData.Width / 2)), Random.Range(-(GameData.Height / 2) + 2, (GameData.Height / 2) - 2));
        waypoint = gm._gm.getSpawnLocation(player);
        GameObject wp = Instantiate(_waypoint, waypoint, Quaternion.identity);
        wp.transform.position = waypoint;
        wps.Add(wp);

        return wp.transform;
        
    }

    public IEnumerator Fleeing(Transform fwp)
    {
        Color currentColor = new Color(225,0,228);
        this.gameObject.GetComponent<AIDestinationSetter>().target = fwp;
        this.gameObject.GetComponent<AILerp>().speed = 2;
        yield return new WaitForSeconds(5f);
        gameObject.GetComponent<SpriteRenderer>().color = currentColor;
        this.gameObject.GetComponent<AILerp>().speed = currentSpeed;
        isFleeing = false;
    }

    public void Flee()
    {
        fleewaypoint = FleeWaypoint();
        isFleeing = true;
        StartCoroutine(Fleeing(fleewaypoint));
    }

    
}
