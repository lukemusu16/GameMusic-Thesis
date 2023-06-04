using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveScore(GameData gd)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath + "/highscores.txt";
        Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveHighscores sh = new SaveHighscores(gd);

        bf.Serialize(stream, sh);
        stream.Close();

    }

    public static SaveHighscores LoadData()
    {
        string path = Application.persistentDataPath + "/highscores.txt";

        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveHighscores sh = bf.Deserialize(stream) as SaveHighscores;
            stream.Close();

            return sh;
        }
        else
        {
            Debug.Log("get good skrub");
            return null;
        }
    }
}
