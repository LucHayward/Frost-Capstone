using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyAttack : MonoBehaviour
{
    public Transform enemyWeaponTransform;
    private Transform playerTransform;

	private GameObject playerGO;
    
    public float range = 5f;
    private float currentTime = 0.0f;
    private float lastAttackTime = 0.0f;
    private float shotDelay = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
		playerGO = GameObject.FindGameObjectWithTag("Player");

        playerTransform = playerGO.GetComponent<Transform>();
    }

	// Update is called once per frame
	void Update()
    {
        currentTime = Time.time;

        if (Vector3.Distance(playerTransform.position, enemyWeaponTransform.position) <= range)
        {
            //Debug.Log("Close enough");
            if (currentTime - lastAttackTime > shotDelay)
            {
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
