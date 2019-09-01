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

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnPlayer();
        StartCoroutine(GameLoop());    
    }

    private void SpawnMeleeEnemies(int numberOfEnemies)
	{
        //meleeEnemies = new EnemyManager[numberOfEnemies];
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
            //END GAME
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
        SpawnMeleeEnemies(10);
        SpawnRangedEnemies(10);
        // TODO update UI
        yield return startWait;
    }

    private IEnumerator RoundPlaying()
    {
        while(ThereIsAPlayer() && ThereIsAEnemy())
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
}
