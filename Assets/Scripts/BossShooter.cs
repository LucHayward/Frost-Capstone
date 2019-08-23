using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossShooter : MonoBehaviour
{
    public GameObject bullet;

    private Enemy enemy;
    private GameObject enemyGO;
    private GameObject playerGO;
    private Player player;


    private Transform enemyTransform;
    private Transform playerTransform;

    public int abLevel;
    
    
    private float currentTime = 0.0f;
    private float lastShotTime = 0.0f;
    private float shotDelay = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = player.GetComponent<Player>();

        enemyGO = GameObject.FindGameObjectWithTag("Enemy");
        enemy = enemy.GetComponent<Enemy>();

        enemyTransform = enemy.transform;
        playerTransform = player.transform;


    }

    // Update is called once per frame
    void Update()
    {

        currentTime = Time.time;
        float dist = Vector3.Distance(playerTransform.position, enemyTransform.position);

        // Check if basic attack is on cooldown (attack speed)
        if (currentTime -  lastShotTime > shotDelay)
        {
            print("shot off cooldown");

            if (dist <= 5.0f)
            {
                print("close enough");
                lastShotTime = currentTime + shotDelay;

                Shoot();
            }
            
        }
        
        
    }

    void Shoot()
    {

        // Calculate the distance between the player and the enemy
        //float dist = Vector3.Distance(playerTransform.position, enemyTransform.position);
        // If close enough, attack player
        //if (dist <= 5.0f)
        //{
        Debug.Log(playerTransform.position.x - enemyTransform.position.x);
        print("Shot Fired");
        Instantiate(bullet, enemyTransform.position, enemyTransform.rotation);
        //}
    }
}
