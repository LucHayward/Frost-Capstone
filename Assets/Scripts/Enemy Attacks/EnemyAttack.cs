using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class EnemyAttack : MonoBehaviour
{
    public Transform enemyWeaponTransform;
    private Transform playerTransform;
    public Animator animator;
	private GameObject playerGO;
    private Enemy enemy;
    
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
		playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO == null)
            Debug.Log("Fuck my titties");
        playerTransform = playerGO.GetComponent<Transform>();
    }

	// Update is called once per frame
	void Update()
    {
        currentTime = Time.time;
        if (Vector3.Distance(playerTransform.position, enemyWeaponTransform.position) <= abilityRange)
        {
            enemy.velocityMagnitude = 0;

            if (currentTime - lastAbilityTime > abilityCD)
            {

                
                //enemy.ability();
                lastAbilityTime = currentTime + abilityCD;


            }

        }

        if (Vector3.Distance(playerTransform.position, enemyWeaponTransform.position) <= range)
        {
            enemy.velocityMagnitude = 0;
            
            //Debug.Log("Close enough");
            if (currentTime - lastAttackTime > shotDelay)
            {
                
                animator.SetTrigger("atk");
                attack();
                lastAttackTime = currentTime + shotDelay;

                
            }

        }

    }

	void attack()
    {
        // Calculate the distance between the player and the enemy
        
        // If close enough, attack player
		Debug.Log("Attack");
        
        


    }

    
}
