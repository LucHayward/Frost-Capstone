using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton Class controlling the general Game state and providing helper methods to other classes.
/// Maintains the state of all the enemies and players through their managers.
/// </summary>
public class GameManager : MonoBehaviour
{
	private static GameManager instance = null;

	public static GameManager Get()
	{
		if (instance == null)
			instance = (GameManager)FindObjectOfType(typeof(GameManager));
		return instance;
	}
	public PauseMenu pauseMenu;
	public float startDelay = 3f;
	public float endDelay = 3f;
	[HideInInspector] public List<FlockAgent> agents = new List<FlockAgent>();
	public GameObject playerPrefab;
	public PlayerManager[] players;
	public EnemyManager[] enemyTypes;
	public GameObject meleeEnemyPrefab;
    [HideInInspector] public List<EnemyManager> meleeEnemies;
    public GameObject rangedEnemyPrefab;
    [HideInInspector] public List<EnemyManager> rangedEnemies;
    public GameObject bossEnemyPrefab;
    [HideInInspector] public List<EnemyManager> bossEnemies;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private int roundNumber = 0;
    private bool spawnedEnemy = false;
    private Text levelText;
    private Text P1scoreText;
    private Text P2scoreText;
    private Text newAreaText;
    private int P1score = 0;
    private int P2score = 0;
    public bool gameOver = false;
    Vector2 score;
    private float naTextTime = 0.0f;
    private bool graveTextVisible = false;
    private bool castleTextVisible = false;
    private bool covernTextVisible = false;

    private void Start()
    {
        gameOver = false;
        PlayerPrefs.SetInt("score", 0);
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);
        GameObject levelUI = GameObject.Find("LevelUI");
        levelText = levelUI.GetComponentInChildren<Text>();


        GameObject NewAreaUI = GameObject.Find("NewAreaUI");
        newAreaText = NewAreaUI.GetComponentInChildren<Text>();


        SpawnPlayer();
        StartCoroutine(GameLoop());    
    }

		GameObject P1scoreUI = GameObject.Find("P1_ScoreUI");
		P1scoreText = P1scoreUI.GetComponentInChildren<Text>();
		GameObject P2scoreUI = GameObject.Find("P2_ScoreUI");
		P2scoreText = P2scoreUI.GetComponentInChildren<Text>();

		SpawnPlayer();
		StartCoroutine(GameLoop());
	}

	private void SpawnMeleeEnemies(int numberOfEnemies)
	{
		for (int i = 0; i < numberOfEnemies; i++)
		{
			Transform spawnPoint = enemyTypes[0].CalculateSpawnPoint();
			meleeEnemies.Add(new EnemyManager());
			meleeEnemies[i].instanceOfEnemy = Instantiate(meleeEnemyPrefab,
				spawnPoint.position,
				spawnPoint.rotation) as GameObject;

			meleeEnemies[i].Setup(i);
			agents.Add(meleeEnemies[i].GetFlockAgent());
		}
	}

	private int CalcuateNumberOfMeleeEnemies()
	{
		int numberOfEnemies = roundNumber * 3;
		return numberOfEnemies;
	}

	private int CalcuateNumberOfRangedEnemies()
	{
		int numberOfEnemies = roundNumber * 2;
		return numberOfEnemies;
	}

	private int CalculateBossEnemies()
	{
		if (roundNumber % 5 != 0)
			return 1;
		else
			return 0;

	}

	private void SpawnRangedEnemies(int numberOfEnemies)
	{
		for (int i = 0; i < numberOfEnemies; i++)
		{
			Transform spawnPoint = enemyTypes[1].CalculateSpawnPoint();
			rangedEnemies.Add(new EnemyManager());
			rangedEnemies[i].instanceOfEnemy = Instantiate(rangedEnemyPrefab,
				spawnPoint.position,
				spawnPoint.rotation) as GameObject;

			rangedEnemies[i].Setup(i);
			agents.Add(rangedEnemies[i].GetFlockAgent());
		}
	}

	private void SpawnBossEnemy(int numberOfEnemies)
	{
		for (int i = 0; i < numberOfEnemies; i++)
		{
			Transform spawnPoint = enemyTypes[2].CalculateSpawnPoint();
			bossEnemies.Add(new EnemyManager());
			bossEnemies[i].instanceOfEnemy = Instantiate(bossEnemyPrefab,
				spawnPoint.position,
				spawnPoint.rotation) as GameObject;

			bossEnemies[i].Setup(i);
			agents.Add(bossEnemies[i].GetFlockAgent());
		}
	}

	private void SpawnPlayer()
	{
		for (int i = 0; i < players.Length; i++)
		{
			players[i].instanceOfPlayer = Instantiate(playerPrefab,
				players[i].spawnPoint.position,
				players[i].spawnPoint.rotation) as GameObject;
			players[i].playerNumber = i + 1;
			players[i].camera.enabled = true;
			players[i].Setup(i);
		}

		// If there's only one player then disable the second camera and reset the UI values
		if (players.Length == 1)
		{
			//Diasble Cam2 and change viewport on Cam1
			players[0].camera.ResetAspect();
			players[0].camera.rect = new Rect(0, 0, 1, 1);
			players[0].camera.ResetAspect();

			// Remove crosshair 2 and recentre crosshair 1
			var p2Cross = GameObject.Find("P2_CrossHair");
			p2Cross.SetActive(false);
			var p1Cross = GameObject.Find("P1_CrossHair");
			var rt1 = p1Cross.GetComponent<RectTransform>();
			rt1.anchorMax = new Vector2(0.5f, 0.5f);
			rt1.anchorMin = new Vector2(0.5f, 0.5f);
			rt1.pivot = new Vector2(0.5f, 0.5f);
			rt1.anchoredPosition = new Vector2(0, 0);
			rt1.anchoredPosition3D = new Vector2(0, 0);

			// Reset DamageImage1
			var p1damageImage = GameObject.Find("P1_DamageImage");
			var dirt1 = p1damageImage.GetComponent<RectTransform>();
			dirt1.anchorMax = new Vector2(1, 1);
			dirt1.anchorMin = new Vector2(0, 0);
			dirt1.pivot = new Vector2(0.5f, 0.5f);
			dirt1.anchoredPosition = new Vector2(0, 0);
			dirt1.anchoredPosition3D = new Vector2(0, 0);


			GameObject.Find("P2_HealthUI").SetActive(false);
			GameObject.Find("P2_BlueFeUI").SetActive(false);
			GameObject.Find("P2_GreenFeUI").SetActive(false);
			GameObject.Find("P2_RedFeUI").SetActive(false);
			GameObject.Find("P2_ScoreUI").SetActive(false);
		}
	}

	private IEnumerator GameLoop()
	{
		yield return StartCoroutine(RoundStarting());
		yield return StartCoroutine(RoundPlaying());
		yield return StartCoroutine(RoundEnding());

		if (!ThereIsAPlayer())
		{
			if (SceneManager.GetActiveScene().buildIndex == 1)
			{
				SpawnPlayer(); //TODO END GAME
			}

		}
		else
		{
			StartCoroutine(GameLoop());
		}
	}

	private IEnumerator RoundStarting()
	{
		ResetAllPlayers();
		roundNumber++;
		levelText.text = "LEVEL " + roundNumber;
		spawnedEnemy = false;
		// TODO update UI
		yield return startWait;
	}

	private IEnumerator RoundPlaying()
	{
		SpawnMeleeEnemies(CalcuateNumberOfMeleeEnemies());
		SpawnRangedEnemies(CalcuateNumberOfRangedEnemies());
		SpawnBossEnemy(CalculateBossEnemies());
		spawnedEnemy = true;
		while (ThereIsAPlayer() && ThereIsAEnemy())
		{
			yield return null;
		}
		if (ThereIsAPlayer() && !ThereIsAEnemy())
		{
			yield return StartCoroutine(GameLoop());
		}
	}

	private IEnumerator RoundEnding()
	{
		yield return endWait;
	}

	private void ResetAllPlayers()
	{
		for (int i = 0; i < players.Length; i++)
		{
			players[i].RoundReset();
		}
	}

	private bool ThereIsAPlayer()
	{
		if (players.Length != 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool ThereIsAEnemy()
	{
		int numberOfNull = 0;
		foreach (FlockAgent agent in agents)
		{
			if (agent == null)
				numberOfNull++;
		}
		if (numberOfNull == agents.Count)
		{
			Debug.Log("Returned False");
			return false;
		}

		else
			return true;
	}

	private void Update()
	{
        float currentTime = Time.time;
		float x = Time.timeScale;
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (pauseMenu.isPaused)
			{
				Time.timeScale = 1;
				pauseMenu.ResumeGame();
			}
			else
			{
				pauseMenu.PauseGame();
				Time.timeScale = 0;
			}
		}

        score = GetScore();

        if(score[0]+score[1] >= 80)
        {
            if(graveTextVisible == false)
            {
                
                newAreaText.text = "The Graveyard is now unlockable";
                naTextTime = currentTime + 6.0f;
                graveTextVisible = true;
            }
            else
            {
                if (currentTime - naTextTime >= 6.0f)
                {
                    newAreaText.text = "";
                }
            }
            
        }

        if (score[0] + score[1] >= 150)
        {
            if (castleTextVisible == false)
            {
                newAreaText.text = "The Castle is now unlockable";
                naTextTime = currentTime + 6.0f;
                castleTextVisible = true;
            }
            else
            {
                if (currentTime - naTextTime >= 6.0f)
                {
                    newAreaText.text = "";
                }
            }

        }

        if (score[0] + score[1] >= 250)
        {
            if (covernTextVisible == false)
            {
                newAreaText.text = "The Enchanted Covern is now unlockable";
                naTextTime = currentTime + 6.0f;
                covernTextVisible = true;
            }
            else
            {
                if (currentTime - naTextTime >= 6.0f)
                {
                    newAreaText.text = "";
                }
            }

        }


    }

	}

	public void UpdateScore(int points, int playerNum)
	{
		if (playerNum == 0)
		{
			P1score += points;
			P1scoreText.text = "SCORE: " + P1score;
		}
		else
		{
			P2score += points;
			P2scoreText.text = "SCORE: " + P2score;
		}
	}

	public void RemovedDeadEnemy(int ID, string type)
	{
		switch (type)
		{
			case "Boss":
				{
					foreach (EnemyManager boss in bossEnemies)
					{
						if (boss.GetID() == ID)
						{
							bossEnemies.Remove(boss);
							break;
						}
					}
					break;
				}
			case "Ranged":
				{
					foreach (EnemyManager ranged in rangedEnemies)
					{
						if (ranged.GetID() == ID)
						{
							rangedEnemies.Remove(ranged);
							break;
						}
					}
					break;
				}
			case "Melee":
				{
					foreach (EnemyManager melee in meleeEnemies)
					{
						if (melee.GetID() == ID)
						{
							meleeEnemies.Remove(melee);
							break;
						}
					}
					break;
				}
		}
	}

	public Vector2 GetScore()
	{
		return new Vector2(P1score, P2score);
	}

	public void HandlePlayerDeath(int playerNum)
	{
		//TODO: Handle multiplayer variant
		if (players.Length == 1)
		{
			PlayerPrefs.SetInt("P1score", P1score);
			PlayerPrefs.SetInt("P2score", P2score);

			SceneManager.LoadScene(2);

			gameOver = true;

		}
		else
		{
			// If neither player is alive now
			if (!(players[0].GetPlayerScript().IsAlive() || players[1].GetPlayerScript().IsAlive()))
			{

				PlayerPrefs.SetInt("P1score", P1score);
				PlayerPrefs.SetInt("P2score", P2score);

				SceneManager.LoadScene(2);

				gameOver = true;

			}

		}
	}

	public Tuple<float, Transform> GetClosestPlayer(Transform transform)
	{
		float shortestDistance = float.MaxValue;
		Transform closestPlayerTransform = null;
		Transform[] playerTransforms = new Transform[this.players.Length];

		for (int i = 0; i < this.players.Length; i++)
		{
			playerTransforms[i] = this.players[i].instanceOfPlayer.transform;
		}

		Player[] players = new Player[this.players.Length];
		for (int i = 0; i < this.players.Length; i++)
		{
			players[i] = this.players[i].GetPlayerScript();
		}

		for (int i = 0; i < playerTransforms.Length; i++)
		{
			if (players[i].IsAlive()) // If the player is dead stop targeting.
			{
				float newDistance = Vector3.Distance(transform.position, playerTransforms[i].position);
				if (newDistance < shortestDistance)
				{
					closestPlayerTransform = playerTransforms[i];
					shortestDistance = newDistance;
				}
			}
		}
		return Tuple.Create(shortestDistance, closestPlayerTransform);
	}
}
