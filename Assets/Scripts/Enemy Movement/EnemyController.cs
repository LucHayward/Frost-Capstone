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
    private Transform playerTrasnform;
    [SerializeField]private FlockAgent flockAgent;

    void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
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
        float distance = Vector3.Distance(transform.position, playerGameObject.transform.position);
        if (distance > 1)
        {
            flockAgent.enabled = false;
            agent.isStopped = false;
            agent.SetDestination(playerTrasnform.position);
        }
        else
        {
            agent.isStopped = true;
            flockAgent.enabled = true;
        }  
    }
    /// <summary>
    /// Controls the movment of the NPC for when they cannot see the player.
    /// </summary>
    private void Wander()
    {
        float distance = Vector3.Distance(transform.position, playerGameObject.transform.position);
        if(distance < 5)
        {
            flockAgent.enabled = false;
            agent.SetDestination(playerTrasnform.position);
        }
        else
        {
            agent.isStopped = true;
            flockAgent.enabled = true;
        }
    }
    /// <summary>
    /// Determines whether the NPC can see the player and makes decisions based on this information
    /// </summary>
    private void MakeDecision()
    {
        Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 directionToPlayer = playerTrasnform.position - currentPosition; // vector pointing from the enemy to the player       
        Ray eyeLine = new Ray(transform.position, directionToPlayer);
        Debug.DrawRay(currentPosition, directionToPlayer);
        int layerMask = LayerMask.GetMask("Player", "Obstacle"); // this is not working.
        // TODO fix the line of sight
        if (Physics.Raycast(eyeLine, out RaycastHit hit, layerMask))
        {
            if (hit.collider.tag.Equals("Player"))
            {
                Move();
            }
            else
            {
                Wander();
            }
        }
        else
        {
            Wander();
        }
    }
}
