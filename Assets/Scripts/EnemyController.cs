using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    private GameObject playerGO;
    private Player player;
    private Transform playerPos;
    // Update is called once per frame

    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        playerPos = playerGO.transform;
    }

    void Update()
    {
        agent.SetDestination(playerPos.position); // moves the enemy to the players position
        // TODO: Account for other enemies in the scene, account for a second player, have different terrain for different enemy types.
    }
}
