using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossShooter : MonoBehaviour
{
    public GameObject bullet;
    

    public Transform enemyTransform;
    public Transform playerTransform;

    private Enemy enemy;
    private GameObject enemyGO;
    private GameObject playerGO;
    private Player player;


    public float coolDown;
    public int abLevel;
    
    
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

        enemyTransform = enemy.transform;
        playerTransform = player.transform;


    }

    // Update is called once per frame
    void Update()
    {

        currentTime = Time.time;
        
        // Check if basic attack is on cooldown (attack speed)
        if (currentTime -  lastShotTime > shotDelay)
        {
            lastShotTime = currentTime + coolDown;
            Shoot();
        }
        
        
    }

    void Shoot()
    {

        // Calculate the distance between the player and the enemy
        float dist = Vector3.Distance(playerTransform.position, enemyTransform.position);
        // If close enough, attack player
        if (dist < 4.5f)
        {
            Debug.Log(playerTransform.position.x - enemyTransform.position.x);
            print("Shot Fired");
            Instantiate(bullet, enemyTransform.position, enemyTransform.rotation);
        }
    }
}
