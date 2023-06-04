using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float playerSpeed = 12f;

    private bool isLockHorizontal;
    private bool isLockVertical;

    [SerializeField]
    Rigidbody2D rb;
    Vector2 movement;

    GameManager manager;
    Animator animator;
    FMODUnity.StudioEventEmitter emitter;

    private bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = 0.25f;



    private IEnumerator MovePlayer(Vector3 dir)
    { 
        isMoving= true;

        float elapsedTime = 0f;

        origPos = transform.position;
        targetPos = origPos + dir;

        if (!manager._gm.isTileAvailable(targetPos))
        {
            isMoving = false;
        }
        else
        {
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;

            isMoving = false;
        }

        
    }


    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.up));
        }

        if (Input.GetKey(KeyCode.A) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.left));
        }

        if (Input.GetKey(KeyCode.S) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.down));
        }

        if (Input.GetKey(KeyCode.D) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.right));
        }

        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("North", true);
            animator.SetBool("South", false);
            animator.SetBool("East", false);
            animator.SetBool("West", false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("North", false);
            animator.SetBool("South", false);
            animator.SetBool("East", false);
            animator.SetBool("West", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("North", false);
            animator.SetBool("South", true);
            animator.SetBool("East", false);
            animator.SetBool("West", false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("North", false);
            animator.SetBool("South", false);
            animator.SetBool("East", true);
            animator.SetBool("West", false);
        }
        else 
        {
            animator.SetBool("North", false);
            animator.SetBool("South", false);
            animator.SetBool("East", false);
            animator.SetBool("West", false);
        }

        CalculateIntensity();
    }

    private void ReduceHealth(int value)
    {
        GameData.Health -= value;
        Text health = GameObject.Find("Canvas").transform.GetChild(1).GetComponentInChildren<Text>();

        health.text = GameData.Health.ToString();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (!col.gameObject.GetComponent<mySeeker>().isFleeing)
            {
                ReduceHealth(1);

                if (GameData.Health <= 0)
                {
                    GameData gd = new GameData();
                    SaveSystem.SaveScore(gd);
                    manager.WriteToFile(manager.timer.text, GameData.Score.ToString(), GameData.Health.ToString());
                    SceneManager.LoadScene("Highscores");
                }
            }

            Destroy(col.gameObject);

            Vector3 newPos = manager._gm.getSpawnLocation(gameObject);

            print(newPos);

            Instantiate(manager._seekerPrefab, newPos, Quaternion.identity);
        }
    }

    private void CalculateIntensity()
    {
        /*GameObject[] seekers = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] food = GameObject.FindGameObjectsWithTag("Food");

        float closestDist;

        float seeker1Dist = Vector3.Distance(seekers[0].transform.position, transform.position);
        float seeker2Dist = Vector3.Distance(seekers[1].transform.position, transform.position);


        if (seeker1Dist < seeker2Dist)
        {
            closestDist = seeker1Dist;
        }
        else if (seeker2Dist < seeker1Dist)
        {
            closestDist = seeker2Dist;
        }
        else
        {
            closestDist = seeker1Dist;
        }

        float calculation = closestDist;

        float clampedCalc = Mathf.Clamp(calculation, 0, 100);
        print(clampedCalc);*/

        float progress = ((float)manager.checkEaten/ (float)manager.foods.Count) * 100;
        emitter.SetParameter("Coins", progress);

        
    }


}
