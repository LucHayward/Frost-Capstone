using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [HideInInspector]public List<FlockAgent> agents = new List<FlockAgent>();
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
    private Text scoreText;
    private int score = 0;
    public bool gameOver = false;

    private void Start()
    {
        gameOver = false;
        PlayerPrefs.SetInt("score", 0);
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);
        GameObject levelUI = GameObject.Find("LevelUI");
        levelText = levelUI.GetComponentInChildren<Text>();
        GameObject scoreUI = GameObject.Find("ScoreUI");
        scoreText = scoreUI.GetComponentInChildren<Text>();
        SpawnPlayer();
        StartCoroutine(GameLoop());    
    }

    private void SpawnMeleeEnemies(int numberOfEnemies)
	{
        for(int i = 0; i <numberOfEnemies; i++)
		{
            Transform spawnPoint = enemyTypes[0].CalculateSpawnPoint();
            meleeEnemies.Add(new EnemyManager());
            meleeEnemies[i].instanceOfEnemy = Instantiate(meleeEnemyPrefab,
                spawnPoint.position,
                spawnPoint.rotation) as GameObject;
			
			meleeEnemies [i].Setup(i);
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
        for(int i = 0; i < players.Length; i++)
        {
            players[i].instanceOfPlayer = Instantiate(playerPrefab,
                players[i].spawnPoint.position,
                players[i].spawnPoint.rotation) as GameObject;
            players[i].playerNumber = i + 1;
            players[i].Setup(i);
        }
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if(!ThereIsAPlayer())
        {
            if(SceneManager.GetActiveScene().buildIndex == 1)
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
        if(ThereIsAPlayer() && !ThereIsAEnemy())
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
        for(int i = 0; i < players.Length; i++)
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
        foreach(FlockAgent agent in agents)
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

        
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "SCORE " + score;
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

    public int GetScore()
    {
        return score;
    }
}
