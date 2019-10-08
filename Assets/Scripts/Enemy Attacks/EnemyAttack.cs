using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class EnemyAttack : MonoBehaviour
{
    public Transform enemyWeaponTransform;
    private Transform enemyTransform;

    public Animator animator;
    private Enemy enemy;
    private GameObject[] playerGOs;
    private Player[] players;
    private Transform[] playerTransforms;

    private EnemyController enemyController;
    private FlockAgent flockAgent;
    public NavMeshAgent navMeshAgent;

    // Ranges
    public float range = 0.0f;
    public float abilityRange = 0.0f;

    // Cooldowns
    public float shotDelay = 0.0f;
    public float abilityCD = 0.0f;

    // Attack Timers
    private float lastAttackTime = 0.0f;
    private float lastAbilityTime = 0.0f;

    private float currentTime = 0.0f;

    // 0 jumpAttack, 1 taunt
    private int abilityCounter = 0;
    
    // used for boss combo
    private int attackCounter = 0;

    private Vector3 closestPlayerPosition;
    private int closestPlayerIndex;
    

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        enemyTransform = enemy.GetComponent<Transform>();

        enemyController = gameObject.GetComponent<EnemyController>();
        flockAgent = gameObject.GetComponent<FlockAgent>();

        playerGOs = GameObject.FindGameObjectsWithTag("Player");
        playerTransforms = new Transform[playerGOs.Length];
        for (int i = 0; i < playerGOs.Length; i++)
        {
            playerTransforms[i] = playerGOs[i].GetComponent<Transform>();
        }
        players = new Player[playerGOs.Length];
        for (int i = 0; i < playerGOs.Length; i++)
        {
            players[i] = playerGOs[i].GetComponent<Player>();
        }
        
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
                    closestPlayerIndex = i;
                    closestPlayerTransform = playerTransforms[i];
                    shortestDistance = newDistance;
                }
            }
        }
        return Tuple.Create(shortestDistance, closestPlayerTransform);
    }

    // Update is called once per frame
    void Update()
    {
        closestPlayerPosition = GetClosestPlayer().Item2.position;
        currentTime = Time.time;



        if (Vector3.Distance(closestPlayerPosition, enemyTransform.position) < range)
        {
            //Debug.Log("Close enough");
            if (currentTime - lastAttackTime > shotDelay)
            {
                if(attackCounter != 4)
                {
                    animator.SetTrigger("atk");
                    lastAttackTime = currentTime + shotDelay;
                    attackCounter ++;
                }

                // if 5th attack then combo
                else
                {
                    animator.SetTrigger("combo");
                    lastAttackTime = currentTime + shotDelay;

                    attackCounter = 0;
                }
                

            }

        }
        else if (Vector3.Distance(closestPlayerPosition, enemyTransform.position) < abilityRange)
        {
          

            if (currentTime - lastAbilityTime > abilityCD)
            {
                if(abilityCounter == 0)
                {
                    animator.SetTrigger("ability");
                    lastAbilityTime = currentTime + abilityCD;
                    abilityCounter++;
                }

                else
                {
                        animator.SetTrigger("taunt1");
                        lastAbilityTime = currentTime + abilityCD;
                        abilityCounter = 0;
                }
                
                


            }
            

        }
        
        


    }

    void attackStart()
    {
        enemy.cantMove = true;
    }

	void attackDamage()
    {

        // Calculate the distance between the player and the enemy
        float dist = Vector3.Distance(closestPlayerPosition, enemyWeaponTransform.position);

        if (dist <= 3.0f && enemy.type != "Ranged")
        {
            players[closestPlayerIndex].TakeDamage(enemy.damage);
        }
        // If close enough, attack player
		//Debug.Log("Attack");
    }

    void attackEnd()
    {
        enemy.cantMove = false;

    }


    

    
}
