using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System.IO;

public enum GameDiff
{ 
	Easy,
	Medium,
	Hard,
}


public class GameManager : MonoBehaviour
{
    private Camera _cam;
    
	[SerializeField] 
	private GameObject _tilePrefab;

	[SerializeField]
	public GameObject _seekerPrefab;

	[SerializeField]
	private GameObject _playerPrefab;

	[SerializeField]
	private GameObject _foodPrefab;

	public GridManager _gm;

	List<GameObject> skrs = new List<GameObject>();

	List<GameObject> obst = new List<GameObject>();

	public List<GameObject> foods = new List<GameObject>();

	public int checkEaten = 0;

	bool levelComplete = false;


	public Text timer;
    private bool isRunning;
    private float elapsedTime = 0f;

	Vector2[,] Points;
	List<Sprite> map = new List<Sprite>();
	List<Sprite> floorTiles = new List<Sprite>();
    Sprite BOTTOM;
	

    public void Start()
	{
		try
		{
			SaveHighscores data = SaveSystem.LoadData();

			GameData.HS1 = data.hs1;
			GameData.HS2 = data.hs2;
			GameData.HS3 = data.hs3;
			GameData.HS4 = data.hs4;
			GameData.HS5 = data.hs5;

		}
		catch (System.Exception e)
		{
			print(e);
		}
		
		//Setting the _cam to main camera
		_cam = Camera.main;
		_cam.orthographicSize = GameData.Width / 2;
		_cam.transform.position = new Vector3((GameData.Width/2)-0.5f, (GameData.Height / 2) - 0.5f, -1f);
		//Loading the path finding
		
		//Creating a new grid
		_gm = new GridManager(GameData.Width	, GameData.Height, _tilePrefab);

		GridGraph gg = AstarData.active.data.AddGraph(typeof(GridGraph)) as GridGraph;

		int width = GameData.Width;
		int height = GameData.Height;
		float nodeSize = 1;
		gg.center = new Vector3((width/2)-0.5f, (height/2)-0.5f, 0f);
		gg.SetDimensions(width, height, nodeSize);
		gg.is2D = true;
		gg.collision.use2D= true;
		gg.collision.type = ColliderType.Ray;
		gg.collision.mask = LayerMask.GetMask("Obstacles", "Food", "Enemies");
		gg.name = "test";
		gg.neighbours = NumNeighbours.Four;


		foreach (Sprite tile in Resources.LoadAll<Sprite>("Tiles"))
		{
			map.Add(tile);
        }

        foreach (Sprite tile in Resources.LoadAll<Sprite>("floor"))
        {
            floorTiles.Add(tile);
        }

        Text health = GameObject.Find("Canvas").transform.GetChild(1).GetComponentInChildren<Text>();
		Text score = GameObject.Find("Canvas").transform.GetChild(2).GetComponentInChildren<Text>();
		timer = GameObject.Find("Canvas").transform.GetChild(3).GetComponentInChildren<Text>();


        health.text = GameData.Health.ToString();
		score.text = GameData.Score.ToString();
		timer.text = "00:00.000";

		CreateMaze();

		
		for (int i = 0; i < 2; i++)
		{
			addSeeker();
		}

		for (int i = 0; i < UnityEngine.Random.Range(10, 20); i++)
		{
			addFood();
		}

		addPlayer();

		startSeeking();

		
		StartStopwatch();
		
	}



	private void Update()
	{
        Stopwatch();
        AstarPath.active.Scan();

		if (!levelComplete)
		{
			foreach (GameObject food in foods)
			{
				if (checkEaten == foods.Count)
				{
					levelComplete = true;
				}
			}
		}
		else
		{
			try
			{
				WriteToFile(timer.text, GameData.Score.ToString(), GameData.Health.ToString());
			}
			catch(Exception e)
			{
				print(e);
			}
            SceneManager.LoadScene(3);
        }
	}

	public void CreateMaze()
	{
		Points = new Vector2[GameData.Width, GameData.Height];

		for (int x = 0; x < GameData.Width; x++)
		{
			for (int y = 0; y < GameData.Height; y++)
			{
				Points[x, y] = new Vector3((1 * x), (1 * y));
				//print(Points[x, y]);
			}
		}

		for (int x = 0; x < GameData.Width; x++)
		{
			for (int y = 0; y < GameData.Height; y++)
			{
				GameObject delSquare;

				if (_gm._tiles.TryGetValue(Points[x, y], out delSquare))
				{
					delSquare.GetComponent<Tile>().setObstacle(true);
				}
			}
		}


		for (int x = 0; x < GameData.Width; x++)
		{
			for (int y = 0; y < GameData.Height; y++)
			{
				if (x == 0 || y == 0 || x == GameData.Width - 1 || y == GameData.Height - 1)
				{
					GameObject delSquare;

					if (_gm._tiles.TryGetValue(Points[x, y], out delSquare))
					{
						delSquare.GetComponent<Tile>().isVisited = true;
					}
				}
			}
		}

		GameObject outSquare;

		//GameData.Width / 2, GameData.Height / 2

		if (_gm._tiles.TryGetValue(Points[GameData.Width / 2, GameData.Height / 2], out outSquare))
		{
			MazeAlgorithm(outSquare, Points);
		}

        for (int x = 0; x < GameData.Width; x++)
        {
            for (int y = 0; y < GameData.Height; y++)
            {
				if (!GetTile(x, y).GetComponent<Tile>().isObstacle())
				{ 
					int rand = UnityEngine.Random.Range(0, floorTiles.Count);
                    GetTile(x, y).GetComponent<SpriteRenderer>().sprite = floorTiles[rand];

                }
            }
        }

    }

	private GameObject GetTile(int x, int y)
	{
        GameObject delSquare;

        if (_gm._tiles.TryGetValue(Points[x, y], out delSquare))
        {
			return delSquare;
        }
		return null;
    }

	private void MazeAlgorithm(GameObject tile, Vector2[,] points)
	{

		tile.GetComponent<Tile>().isVisited = true;

		Vector3 pos = tile.transform.position;


		GameObject[] tiles = new GameObject[4];


		if (_gm._tiles.TryGetValue(points[(int)pos.x+1, (int)pos.y], out tiles[0]) &&
			_gm._tiles.TryGetValue(points[(int)pos.x, (int)pos.y+1], out tiles[1]) &&
			_gm._tiles.TryGetValue(points[(int)pos.x-1, (int)pos.y], out tiles[2]) &&
			_gm._tiles.TryGetValue(points[(int)pos.x, (int)pos.y-1], out tiles[3]))
		{
			int ranNum = UnityEngine.Random.Range(0, 4);

			while (tiles[0].GetComponent<Tile>().isVisited == false ||
				  tiles[1].GetComponent<Tile>().isVisited == false ||
				  tiles[2].GetComponent<Tile>().isVisited == false ||
				  tiles[3].GetComponent<Tile>().isVisited == false)
			{

				if (tiles[ranNum].GetComponent<Tile>().isVisited == false)
				{
					switch (ranNum)
					{
						case 0:
							tiles[0].GetComponent<Tile>().isVisited = true;
							tiles[1].GetComponent<Tile>().isVisited = true;
							tiles[3].GetComponent<Tile>().isVisited = true;

							tiles[ranNum].GetComponent<Tile>().setObstacle(false);

							MazeAlgorithm(tiles[ranNum], points);
							break;

						case 1:
							tiles[1].GetComponent<Tile>().isVisited = true;
							tiles[0].GetComponent<Tile>().isVisited = true;
							tiles[2].GetComponent<Tile>().isVisited = true;

							tiles[ranNum].GetComponent<Tile>().setObstacle(false);

							MazeAlgorithm(tiles[ranNum], points);
							break;

						case 2:
							tiles[2].GetComponent<Tile>().isVisited = true;
							tiles[1].GetComponent<Tile>().isVisited = true;
							tiles[3].GetComponent<Tile>().isVisited = true;

							tiles[ranNum].GetComponent<Tile>().setObstacle(false);

							MazeAlgorithm(tiles[ranNum], points);
							break;

						case 3:
							tiles[3].GetComponent<Tile>().isVisited = true;
							tiles[0].GetComponent<Tile>().isVisited = true;
							tiles[2].GetComponent<Tile>().isVisited = true;

							tiles[ranNum].GetComponent<Tile>().setObstacle(false);

							MazeAlgorithm(tiles[ranNum], points);
							break;
					}
				}
				else if (ranNum == 3)
				{
					ranNum = 0;
				}
				else
				{
					ranNum++;
				}
			}
		}


		return;



	}

	private void addSeeker()
	{
		Vector3 spawnLoc = _gm.getSpawnLocation();
		GameObject seek = Instantiate(_seekerPrefab, spawnLoc, Quaternion.identity);
		skrs.Add(seek);
	}

	private void addPlayer()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		//Vector3 spawnLoc = (_gm.getSpawnLocation(enemies[0]) + _gm.getSpawnLocation(enemies[1]))/2;
		Vector3 spawnLoc = _gm.getSpawnLocation();

        GameObject player = Instantiate(_playerPrefab, spawnLoc, Quaternion.identity);
	}

	public void addObstacle(Vector2 spawnLoc)
    {
		GameObject tile = _tilePrefab;

        GameObject bl = Instantiate(tile, spawnLoc, Quaternion.identity);

        bl.GetComponent<Tile>().setObstacle(true);

		obst.Add(bl);

    }

	private void addFood()
	{
		GameObject food = _foodPrefab;

		Vector3 spawnLoc = _gm.getSpawnLocation();


		if (_gm._tiles.ContainsKey(new Vector2Int((int)spawnLoc.x, (int)spawnLoc.y)))
		{
			GameObject bl = Instantiate(food, spawnLoc, Quaternion.identity);

			foods.Add(bl);


		}

	}

	private void startSeeking()
	{
		foreach (GameObject s in skrs)
		{
			s.GetComponent<mySeeker>().hasStarted = true;
		}

	}

    private void stopNClear()
    {
		foreach(GameObject s in skrs)
		{
			Destroy(s);
		}
		skrs.RemoveRange(0, skrs.Count);

		foreach (GameObject tile in obst)
		{
			Destroy(tile);
		}
		obst.RemoveRange(0, obst.Count);

	}

	private void Stopwatch()
	{
        if (isRunning)
        {
            // Update the elapsed time and display it in the text object
            elapsedTime += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            timer.text = string.Format("{0:00}:{1:00}.{2:000}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }
    }

    public void StartStopwatch()
    {
        isRunning = true;
    }

    // Stop the stopwatch
    public void StopStopwatch()
    {
        isRunning = false;
    }

    public void ResetStopwatch()
    {
        elapsedTime = 0f;
        timer.text = "00:00.000";
    }

	public void WriteToFile(string time, string score, string lives)
	{
        string filePath = Application.persistentDataPath + "/data.csv";

        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(time + "," + score + "," + lives);
        }
    }


}
