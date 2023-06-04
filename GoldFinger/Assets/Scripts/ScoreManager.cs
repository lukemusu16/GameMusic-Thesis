using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    
    Text hs1;
    Text hs2;
    Text hs3;
    Text hs4;
    Text hs5;

    // Start is called before the first frame update
    void Start()
    {
        SaveHighscores data = SaveSystem.LoadData();

        hs1 = GameObject.Find("Canvas").transform.GetChild(5).GetComponent<Text>();
        hs2 = GameObject.Find("Canvas").transform.GetChild(6).GetComponent<Text>();
        hs3 = GameObject.Find("Canvas").transform.GetChild(7).GetComponent<Text>();
        hs4 = GameObject.Find("Canvas").transform.GetChild(8).GetComponent<Text>();
        hs5 = GameObject.Find("Canvas").transform.GetChild(9).GetComponent<Text>();

        hs1.text = data.hs1.ToString();
        hs2.text = data.hs2.ToString();
        hs3.text = data.hs3.ToString();
        hs4.text = data.hs4.ToString();
        hs5.text = data.hs5.ToString();

        Debug.Log(data.hs1);
        Debug.Log(data.hs2);
        Debug.Log(data.hs3);
        Debug.Log(data.hs4);
        Debug.Log(data.hs5);
    }
}
