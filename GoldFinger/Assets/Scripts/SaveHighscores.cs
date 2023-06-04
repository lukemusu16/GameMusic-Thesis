using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveHighscores
{
    public int hs1;
    public int hs2;
    public int hs3;
    public int hs4;
    public int hs5;

    public SaveHighscores(GameData gd)
    {
        hs1 = gd.Highscore1;
        hs2 = gd.Highscore2;
        hs3 = gd.Highscore3;
        hs4 = gd.Highscore4;
        hs5 = gd.Highscore5;
    }
}
