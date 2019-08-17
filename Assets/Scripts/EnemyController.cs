using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///  A class that handles the enemy non-player character's movement
/// Contains all methods for performing the basic AI and path finding
/// </summary>


public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    private GameObject playerGameObject;
    private Player player;
    private Transform playerTrasnform;

    void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        player = playerGameObject.GetComponent<Player>();
        playerTrasnform = playerGameObject.transform;
    }

    private void Update()
    {
        MakeDecision();
    }

    /// <summary>
    /// Moves the current transform to the player at a variable speed
    /// </summary>
    private void Move()
    {
        agent.SetDestination(playerTrasnform.position);
    }
    /// <summary>
    /// Controls the movment of the NPC for when they cannot see the player.
    /// </summary>
    private void Wander()
    {
        float distance = Vector3.Distance(playerTrasnform.position, transform.position);
        if(distance > 10)
        {
            agent.SetDestination(transform.position);
        }
        else
        {
            agent.SetDestination(playerTrasnform.position);
        }

    }
    /// <summary>
    /// Determines whether the NPC can see the player and makes decisions based on this information
    /// </summary>
    private void MakeDecision()
    {
        Vector3 directionToPlayer = playerTrasnform.position - transform.position; /// vector pointing from the enemy to the player
        Ray eyeLine = new Ray(transform.position, directionToPlayer);
        Debug.DrawRay(transform.position, directionToPlayer);
        if (Physics.Raycast(eyeLine, out RaycastHit hit))
        {
            if (hit.collider.tag.Equals("Player"))
            {
                Debug.Log("Enemy sees player");
                Move();

            }
            else
            {
                Debug.Log("Enemy cannot see player but sees other object");
                Wander();
            }
        }
        else
        {
            Debug.Log("Enemy cannot see player");
            Wander();
        }
    }
}
