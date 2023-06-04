[System.Serializable]
public class GameData
{
    private static int _score;
    private static int _height = 30;
    private static int _width = 30;
    private static float _offset =_height/2;
    private static int _health = 3;
    private static float _volume = 1;

    private static int hs1;
    private static int hs2;
    private static int hs3;
    private static int hs4;
    private static int hs5;

    public int _gamescore;
    public int Highscore1;
    public int Highscore2;
    public int Highscore3;
    public int Highscore4;
    public int Highscore5;

    private static GameDiff _currentDiff;

    public static float Volume
    {
        get { return _volume; }
        set { _volume = value; }
    }

    public static int Score
    {
        get { return _score; }
        set { _score = value; }
    }

    public static int Height
    {
        get { return _height; }
    }

    public static int Width
    {
        get { return _width; }
    }

    public static float Offset
    {
        get { return _offset; }
    }

    public static int Health
    {
        get { return _health; }
        set { _health = value; }
    }


    public static int HS1
    {
        get { return hs1; }
        set { hs1 = value; }
    }

    public static int HS2
    {
        get { return hs2; }
        set { hs2 = value; }
    }

    public static int HS3
    {
        get { return hs3; }
        set { hs3 = value; }
    }
    public static int HS4
    {
        get { return hs4; }
        set { hs4 = value; }
    }
    public static int HS5
    {
        get { return hs5; }
        set { hs5 = value; }
    }

    public static GameDiff CurrentDiff
    {
        get { return _currentDiff; }
        set { _currentDiff = value; }
    }
    
    public GameData()
    {
        _gamescore = Score;


        if (Score > HS1)
        {
            Highscore5 = HS4;
            Highscore4 = HS3;
            Highscore3 = HS2;
            Highscore2 = HS1;
            Highscore1 = Score;

        }
        else if (HS1 > Score && Score > HS2)
        {
            Highscore5 = HS4;
            Highscore4 = HS3;
            Highscore3 = HS2;
            Highscore2 = Score;

            Highscore1 = HS1;

        }
        else if (HS2 > Score && Score > HS3)
        {
            Highscore5 = HS4;
            Highscore4 = HS3;
            Highscore3 = Score;

            Highscore1 = HS1;
            Highscore2 = HS2;

        }
        else if (HS3 > Score && Score > HS4)
        {
            Highscore5 = HS4;
            Highscore4 = Score;

            Highscore1 = HS1;
            Highscore2 = HS2;
            Highscore3 = HS3;

        }
        else if (HS4 > Score && Score > HS5)
        {
            Highscore5 = Score;

            Highscore1 = HS1;
            Highscore2 = HS2;
            Highscore3 = HS3;
            Highscore4 = HS4;
        }
        else
        {
            Highscore1 = HS1;
            Highscore2 = HS2;
            Highscore3 = HS3;
            Highscore4 = HS4;
            Highscore5 = HS5;
        }
    }


}
