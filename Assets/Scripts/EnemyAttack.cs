using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyAttack : MonoBehaviour
{
	private Transform enemyPos;
	private Transform playerPos;

	private Enemy enemy;
	private GameObject enemyGO;
	private GameObject playerGO;
    private Player player;

    public float coolDown;
    public int abLevel;
    public float waitTime;
    public float abStart;
    public float currentTime;
    public float lastShotTime = 0.0f;
    public float shotDelay;

    // Start is called before the first frame update
    void Start()
    {
		playerGO = GameObject.FindGameObjectWithTag("Player");
		player = player.GetComponent<Player>();

		enemyGO = GameObject.FindGameObjectWithTag("Enemy");
		enemy = enemy.GetComponent<Enemy>();

		enemyPos = enemy.transform;
		playerPos = player.transform;
	}

	// Update is called once per frame
	void Update()
    {
        currentTime = Time.time;
        
        // Check if basic attack is on cooldown (attack speed)
        if (currentTime -  lastShotTime > shotDelay)
        {
            lastShotTime = currentTime + coolDown;
            attack();
        }
        
    }

	void attack()
    {
        // Calculate the distance between the player and the enemy
        float dist = Vector3.Distance(playerPos.position, enemyPos.position);
        // If close enough, attack player
        if (dist < 1.5f)
        {
			Debug.Log(playerPos.position.x - enemyPos.position.x);
            print("Attack");
            player.health--;
        }
    }
}
