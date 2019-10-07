using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class EnemyAttack : MonoBehaviour
{
    public Transform enemyWeaponTransform;
    private Transform enemyTransform;

    private Transform playerTransform;
    public Animator animator;
	private GameObject playerGO;
    private Enemy enemy;
    private Player player;

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
    

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        enemyTransform = enemy.GetComponent<Transform>();

        enemyController = gameObject.GetComponent<EnemyController>();
        flockAgent = gameObject.GetComponent<FlockAgent>();

        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGO.GetComponent<Transform>();
        player = playerGO.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;

        

        if (Vector3.Distance(playerTransform.position, enemyTransform.position) < range)
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
        else if (Vector3.Distance(playerTransform.position, enemyTransform.position) < abilityRange)
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
        float dist = Vector3.Distance(playerTransform.position, enemyWeaponTransform.position);

        if (dist <= 3.0f && enemy.type != "Ranged")
        {
            player.TakeDamage(enemy.damage);
        }
        // If close enough, attack player
		//Debug.Log("Attack");
    }

    void attackEnd()
    {
        enemy.cantMove = false;

    }


    

    
}
