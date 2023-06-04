using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{

    int points;

    private bool isEaten = false;

    GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetIsEaten()
    {
        return isEaten;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameObject[] skrs = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject seeker in skrs)
            {
                seeker.GetComponent<mySeeker>().Flee();
            }

            points = Random.Range(1, 5) * 5;
            GameData.Score += points;

            Text score = GameObject.Find("Canvas").transform.GetChild(2).GetComponentInChildren<Text>();

            score.text = GameData.Score.ToString();

            isEaten = true;
            gm.checkEaten++;
            Destroy(gameObject);
        }
    }
}
