using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Vector3 shotPath;
    public float shotSpeed;

    private GameObject playerGO;
    private Player player;
    private Transform playerPos;
    
    public GameObject rangedEnemyGO;
    public Transform rePos;
    public Enemy rangedEnemy;

    // Start is called before the first frame update
    void Start()
    {
        rangedEnemyGO = GameObject.FindGameObjectWithTag("EnemyRanged");
        rangedEnemy = rePos.GetComponent<Enemy>();
        rePos = rangedEnemy.transform;

        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = player.GetComponent<Player>();
        playerPos = player.transform;

        shotPath = playerPos.position - rePos.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, shotPath*2, shotSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position,shotPath*2) <0.2f)
        {
            Destroy(gameObject);
        }
        
    }
}
