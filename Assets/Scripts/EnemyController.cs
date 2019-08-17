using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    private GameObject playerGameObject;
    private Player player;
    private Transform playerTrasnform;
    // Update is called once per frame

    void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        player = playerGameObject.GetComponent<Player>();
        playerTrasnform = playerGameObject.transform;
    }

    private void Update()
    {

        makeDecision();
    }

    private void move()
    {
        agent.SetDestination(playerTrasnform.position);
    }

    private void wander()
    {
        // move in general direction of enemy
    }

    private void makeDecision()
    {
        // decide what action to take
        Vector3 directionToPlayer = playerTrasnform.position - transform.position; // vector pointing from the enemy to the player
        Ray eyeLine = new Ray(transform.position, directionToPlayer);
        if (Physics.Raycast(eyeLine, out RaycastHit hit))
        {
            if (hit.collider.tag.Equals("Player"))
            {
                Debug.Log("Enemy sees player");
                agent.SetDestination(playerTrasnform.position);

            }
            else if (hit.collider.tag.Equals("Obstacle"))
            {
                Debug.Log("Enemy cannot see player");
                wander();
            }

        }
    }
}
