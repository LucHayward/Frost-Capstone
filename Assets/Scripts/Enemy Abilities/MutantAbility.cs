﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MutantAbility : MonoBehaviour
{

    private GameObject mutantGO;
    private Enemy mutant;
    public Transform weaponTransform;

    private GameObject playerGO;
    private Player player;
    public NavMeshAgent navMeshAgent;

    public Animator animator;
    private EnemyController mutantController;
    private FlockAgent flockAgent;

    private Transform mutantTransform;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        mutantGO = gameObject;
        mutant = gameObject.GetComponent<Enemy>();
        mutantTransform = mutant.GetComponent<Transform>();

        mutantController = mutantGO.GetComponent<EnemyController>();
        flockAgent = mutantGO.GetComponent<FlockAgent>();

        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        playerTransform = player.GetComponent<Transform>();

    }

    // Update is called once per frame
    void jumpAttackStart()
    {
        mutantController.enabled = false;
        flockAgent.enabled = false;
        navMeshAgent.enabled = false;
    }

    void jumpAttackMove()
    {
        mutantController.enabled = true;
        flockAgent.enabled = true;
        navMeshAgent.enabled = true;
    }

    void jumpAttackDamage()
    {
        float dist = Vector3.Distance(playerTransform.position, weaponTransform.position);
        
        if (dist < 3.0f)
        {
            player.TakeDamage(mutant.abilityDamage);
        }

    }

    void jumpAttackLand()
    {
        mutantController.enabled = false;
        flockAgent.enabled = false;
        navMeshAgent.enabled = false;
    }

    void jumpAttackEnd()
    {
        mutantController.enabled = true;
        flockAgent.enabled = true;
        navMeshAgent.enabled = true;
    }
}
