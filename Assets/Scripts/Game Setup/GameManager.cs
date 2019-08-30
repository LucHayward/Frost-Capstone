using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float startDelay = 3f;
    public float endDelay = 3f;

    public GameObject playerPrefab;
    public PlayerManager[] players;

    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private int roundNumber = 0;
    private PlayerManager[] deadPlayers;
    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnPlayer();
        
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
        //yield return StartCoroutine(RoundPLaying());
        //yield return StartCoroutine(RoundEnding());

        if(deadPlayers.Length == players.Length)
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

    //private IEnumerator RoundPLaying()
    //{

    //}

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
    // Update is called once per frame
    void Update()
    {
        
    }
}
