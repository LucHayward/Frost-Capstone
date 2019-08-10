﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyAttack : MonoBehaviour
{
    private Enemy enemy;
    public Transform enemyPos;
    private Player player;
    public Transform playerPos;

    public float coolDown;
    public int abLevel;
    public float shootTime = 0;
    public float waitTime;
    public float abStart;
    public float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        enemyPos = GameObject.FindGameObjectWithTag("Enemy").transform;
        
    }

    // Update is called once per frame
    void Update()
    {

        currentTime = Time.time;
        

        if (Time.time > shootTime)
        {
            shootTime = Time.time + coolDown;
            attack();
        }
        
    }

    void attack()
    {
        if ((playerPos.position.x - enemyPos.position.x) < 10)
        {
            print("Attack");
            player.health--;
        }
    }
}