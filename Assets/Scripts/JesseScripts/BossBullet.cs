using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Vector2 shot;
    public float speed;
    private PlayerJ player;
    private Boss boss;
    private Transform playerPos;
    private Transform bossPos;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        bossPos = GameObject.FindGameObjectWithTag("Boss").transform; ;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerJ>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        shot = playerPos.position - bossPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, shot*2, speed * Time.deltaTime);

        if(Vector2.Distance(transform.position,shot*2) <0.2f)
        {
            Destroy(gameObject);
        }
        
    }
}
