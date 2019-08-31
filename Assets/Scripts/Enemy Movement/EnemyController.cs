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
    //Animator animator;
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
        
        //animator.SetTrigger("walk");
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
        //animator.ResetTrigger("walk");
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
        int layerMask = LayerMask.GetMask("Player", "Obstacle"); // this is not working.
        // TODO fix the line of sight
        if (Physics.Raycast(eyeLine, out RaycastHit hit, layerMask))
        {
            if (hit.collider.tag.Equals("Player"))
            {
                Debug.Log("Sees player");
                //animator.SetTrigger("seePlayer");

                //animator.SetTrigger("run");
                Move();
                
                //animator.SetBool("hasSeen", true);
                
            }
            else
            {
                Debug.Log("Does not see enemy");
                Debug.Log(hit.transform.tag);
                //animator.SetTrigger("walk");
                Wander();
            }
        }
        else
        {
            Wander();
        }
    }
}
