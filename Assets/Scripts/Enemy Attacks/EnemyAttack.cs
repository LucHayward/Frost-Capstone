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

    public float range = 0.0f;
    public float abilityRange = 0.0f;
    public float abilityCD = 0.0f;
    private float lastAbilityTime = 0.0f;

    private float currentTime = 0.0f;
    private float lastAttackTime = 0.0f;
    public float shotDelay = 0.0f;

    

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


                animator.SetTrigger("atk");
                lastAttackTime = currentTime + shotDelay;

                
            }

        }
        else if (Vector3.Distance(playerTransform.position, enemyTransform.position) < abilityRange)
        {

            if (currentTime - lastAbilityTime > abilityCD)
            {
                animator.SetTrigger("ability");
                lastAbilityTime = currentTime + abilityCD;


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

        if (dist <= 2.0f)
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
