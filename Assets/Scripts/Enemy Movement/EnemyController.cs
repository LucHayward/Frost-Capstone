using System;
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
    public Animator animator;
    public NavMeshAgent agent;
    public float stoppingDistance;
    private GameObject[] playerGameObjects;
    private Player[] players;
    private Transform[] playerTransforms;
    private Enemy enemy;
    private bool hasSeen = false;
    [SerializeField]private FlockAgent flockAgent = null; //Assigned in inspector

	void Start()
    {
        playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        playerTransforms = new Transform[playerGameObjects.Length];
        for(int i = 0; i < playerGameObjects.Length; i++)
        {
            playerTransforms[i] = playerGameObjects[i].transform;
        }
        players = new Player[playerGameObjects.Length];
        for (int i = 0; i < playerGameObjects.Length; i++)
        {
            players[i] = playerGameObjects[i].GetComponent<Player>();
        }
        enemy = gameObject.GetComponent<Enemy>();
    }

    private void Update()
    {
        MakeDecision();

        if (enemy.cantMove || enemy.isStunned)
        {
            StopMove();
        }
        
        //else
        //{
        //    //agent.isStopped = false;
        //    //flockAgent.enabled = true;
        //}
    }

    private Tuple<float, Transform> GetClosestPlayer()
    {
        float shortestDistance = float.MaxValue;
        Transform closestPlayerTransform = null;
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

    /// <summary>
    /// Moves the current transform to the closest player at a variable speed USING THE NAVMESH if close to player, otherwise 
    /// </summary>
    private void Move()
	{
        
        if (!enemy.isStunned)
        {
            agent.isStopped = false;
        }
		flockAgent.enabled = false;
        
        
        Tuple<float, Transform> tuple = GetClosestPlayer();
        float shortestDistance = tuple.Item1;
        Transform closestPlayerTransform = tuple.Item2;
        transform.LookAt(closestPlayerTransform.position); //TODO Lerp this (see player movement)


        if (shortestDistance > stoppingDistance)
        {
            //agent.isStopped = false;
            agent.SetDestination(closestPlayerTransform.position);
		}
		//else
		//{
		//    //agent.isStopped = true;
		//    flockAgent.enabled = false;
		//}  
	}
    /// <summary>
    /// Controls the movment of the NPC for when they cannot see the player.
	/// If the agent is close to the player, disable flocking and move to the player USING THE NAVMESH. Otherwise enable the flock agent component
    /// </summary>
    private void Wander()
    {
        Tuple<float, Transform> tuple = GetClosestPlayer();
        float shortestDistance = tuple.Item1;
        Transform closestPlayerTransform = tuple.Item2;
        if (shortestDistance < 5)
        {
            flockAgent.enabled = false;
            agent.SetDestination(closestPlayerTransform.position);
        }
        else
        
            //agent.isStopped = true;
            flockAgent.enabled = true;
        
    }
    /// <summary>
    /// Determines whether the NPC can see the player and makes decisions based on this information
    /// </summary>
    private void MakeDecision()
    {
        Tuple<float, Transform> tuple = GetClosestPlayer();
        float shortestDistance = tuple.Item1;
        Transform closestPlayerTransform = tuple.Item2;


        Vector3 currentPosition = new Vector3(transform.position.x, 1, transform.position.z);
        Vector3 centralizedPlayerPosition = new Vector3(closestPlayerTransform.position.x, 1, closestPlayerTransform.position.z);
        Vector3 directionToPlayer = centralizedPlayerPosition - currentPosition; // vector pointing from the enemy to the player       
        Ray eyeLine = new Ray(currentPosition, directionToPlayer);
        //Debug.DrawRay(currentPosition, directionToPlayer);
        int layerMask = LayerMask.GetMask("Player", "Obstacle");
        // TODO fix the line of sight
        if (Physics.Raycast(eyeLine, out RaycastHit hit, layerMask))
        {
            //Debug.Log(hit.collider.tag);
            if (hit.collider.tag.Equals("Player"))
            {
                hasSeen = true;
                //if(enemy.GetType().Equals("Melee"))
                //{
                //    if (enemy.hasScreamed == false)
                //    {
                //        animator.SetTrigger("scream");
                //        enemy.hasScreamed = true;
                //    }
                //}
                

                Move();                
            }
            
            else
            {
                //prevent enemies wandering when line of sight is blocked by other enemies
                if (hasSeen == false)
                {
                    Wander();
                }
                else
                {
                    Move();
                }   
            }
        }
        else
        {
            Wander();
        }
    }

    
    /// <summary>
    /// Stops the agent from moving
    /// </summary>
    public void StopMove()
    {
        agent.isStopped = true;
        flockAgent.enabled = false;
    }
}
