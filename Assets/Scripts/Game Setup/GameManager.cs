using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float startDelay = 3f;
    public float endDelay = 3f;
    public static List<FlockAgent> agents = new List<FlockAgent>();
    public GameObject playerPrefab;
    public PlayerManager[] players;
    public EnemyManager[] enemyTypes;
	public GameObject meleeEnemyPrefab;
	private EnemyManager[] meleeEnemies;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private int roundNumber = 0;

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnPlayer();
        SpawnMeleeEnemies(10);
        StartCoroutine(GameLoop());    
    }

    private void SpawnMeleeEnemies(int numberOfEnemies)
	{
        meleeEnemies = new EnemyManager[numberOfEnemies];
        for(int i = 0; i <numberOfEnemies; i++)
		{
            enemyTypes[0].CalculateSpawnPoint();
            meleeEnemies[i].instanceOfEnemy = Instantiate(meleeEnemyPrefab,
                enemyTypes[0].spawnPoint.position,
                enemyTypes[0].spawnPoint.rotation) as GameObject;
            meleeEnemies[i].Setup();
            agents.Add(meleeEnemies[i].GetFlockAgent());
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
        yield return StartCoroutine(RoundPLaying());
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
        // TODO update UI


        yield return startWait;
    }

    private IEnumerator RoundPLaying()
    {
        while(ThereIsAPlayer())
        {
            yield return null;
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
}
