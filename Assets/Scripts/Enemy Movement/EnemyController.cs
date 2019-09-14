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
    private GameObject playerGameObject;
    private Transform playerTrasnform;
    private Enemy enemy;
    private bool hasSeen = false;
    [SerializeField]private FlockAgent flockAgent = null; //Assigned in inspector

	void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerTrasnform = playerGameObject.transform;
        enemy = gameObject.GetComponent<Enemy>();
    }

    private void Update()
    {
        MakeDecision();

        if (enemy.cantMove || enemy.isStunned)
        {
            StopMove();
        }
        
        else
        {
            agent.isStopped = false;
            //flockAgent.enabled = true;
        }
    }

    /// <summary>
    /// Moves the current transform to the player at a variable speed
    /// </summary>
    private void Move()
    {
            transform.LookAt(playerTrasnform.position);
            float distance = Vector3.Distance(transform.position, playerGameObject.transform.position);
            if (distance > stoppingDistance)
            {
                flockAgent.enabled = false;
                agent.isStopped = false;
                agent.SetDestination(playerTrasnform.position);
            }
            else
            {
                agent.isStopped = true;
                flockAgent.enabled = false;
            }  
    }
    /// <summary>
    /// Controls the movment of the NPC for when they cannot see the player.
    /// </summary>
    private void Wander()
    {
            float distance = Vector3.Distance(transform.position, playerGameObject.transform.position);
            if (distance < 5)
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
        Vector3 currentPosition = new Vector3(transform.position.x, 1, transform.position.z);
        Vector3 centralizedPlayerPosition = new Vector3(playerTrasnform.position.x, 1, playerTrasnform.position.z);
        Vector3 directionToPlayer = centralizedPlayerPosition - currentPosition; // vector pointing from the enemy to the player       
        Ray eyeLine = new Ray(currentPosition, directionToPlayer);
        Debug.DrawRay(currentPosition, directionToPlayer);
        int layerMask = LayerMask.GetMask("Player", "Obstacle");
        // TODO fix the line of sight
        if (Physics.Raycast(eyeLine, out RaycastHit hit, layerMask))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag.Equals("Player"))
            {
                hasSeen = true;
                if(enemy.GetType().Equals("Melee"))
                {
                    if (enemy.hasScreamed == false)
                    {
                        animator.SetTrigger("scream");
                        enemy.hasScreamed = true;
                    }
                }
                

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

    //TODO: @Keegan @Luc Fix this, can't have both active together
    /// <summary>
    /// Stops the agent from moving
    /// </summary>
    public void StopMove()
    {
        agent.isStopped = true;
        flockAgent.enabled = false;
    }

    /// <summary>
    /// Allows the agent to start moving
    /// </summary>
    public void ResumeMove()
    {
        agent.isStopped = false;
        flockAgent.enabled = true;
    }
}
