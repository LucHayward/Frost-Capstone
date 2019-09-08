using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnPlayer();
        StartCoroutine(GameLoop());    
    }

    private void SpawnMeleeEnemies(int numberOfEnemies)
	{
        for(int i = 0; i <numberOfEnemies; i++)
		{
            enemyTypes[0].CalculateSpawnPoint();
            meleeEnemies.Add(new EnemyManager());
            meleeEnemies[i].instanceOfEnemy = Instantiate(meleeEnemyPrefab,
                enemyTypes[0].spawnPoint.position,
                enemyTypes[0].spawnPoint.rotation) as GameObject;
			
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
        if (roundNumber % 5 != 1)
            return 0;
        else
            return 2;

    }

    private void SpawnRangedEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            enemyTypes[1].CalculateSpawnPoint();
            rangedEnemies.Add(new EnemyManager());
            rangedEnemies[i].instanceOfEnemy = Instantiate(rangedEnemyPrefab,
                enemyTypes[1].spawnPoint.position,
                enemyTypes[1].spawnPoint.rotation) as GameObject;

            rangedEnemies[i].Setup(i);
            agents.Add(rangedEnemies[i].GetFlockAgent());
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
            players[i].Setup();
        }
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if(!ThereIsAPlayer())
        {
            SpawnPlayer(); //TODO END GAME
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
        spawnedEnemy = false;
        // TODO update UI
        yield return startWait;
    }

    private IEnumerator RoundPlaying()
    {
        SpawnMeleeEnemies(CalcuateNumberOfMeleeEnemies());
        SpawnRangedEnemies(CalcuateNumberOfRangedEnemies());
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void ResumeGame()
    {
        pauseMenu.ResumeGame();
        ResumePlayerMovement();
        ResumeEnemyMovement();
    }

    private void PauseGame()
    {
        pauseMenu.PauseGame();
        PausePlayerMovement();
        PauseEnemyMovement();
    }

    private void PausePlayerMovement()
    {
        foreach(PlayerManager player in players)
        {
            player.StopMovment();
        }
    }

    private void ResumePlayerMovement()
    {
        foreach (PlayerManager player in players)
        {
            player.ResumeMovement();
        }
    }

    private void PauseEnemyMovement()
    {
        foreach(EnemyManager meleeEnemy in meleeEnemies)
        {
            meleeEnemy.DisableMovement();
        }

        foreach (EnemyManager rangedEnemy in rangedEnemies)
        {
            rangedEnemy.DisableMovement();
        }

        foreach (EnemyManager bossEnemy in bossEnemies)
        {
            bossEnemy.DisableMovement();
        }
    }

    private void ResumeEnemyMovement()
    {
        foreach (EnemyManager meleeEnemy in meleeEnemies)
        {
            meleeEnemy.EnableMovement();
        }

        foreach (EnemyManager rangedEnemy in rangedEnemies)
        {
            rangedEnemy.EnableMovement();
        }

        foreach (EnemyManager bossEnemy in bossEnemies)
        {
            bossEnemy.EnableMovement();
        }
    }
}
