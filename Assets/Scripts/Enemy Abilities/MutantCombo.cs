﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantCombo : MonoBehaviour
{

    //private GameObject mutantGO;
    private Enemy mutant;
    public Transform weaponTransform;

    private GameObject[] playerGOs;
    private Player[] players;
    private Transform[] playerTransforms;
    private Vector3 closestPlayerPosition;
    private int closestPlayerIndex;


    public Animator animator;
    //private EnemyController mutantController;
    //private FlockAgent flockAgent;

    //private Transform mutantTransform;


    // Start is called before the first frame update
    void Start()
    {
        //mutantGO = gameObject;
        mutant = gameObject.GetComponent<Enemy>();
        //mutantTransform = mutant.GetComponent<Transform>();

        //mutantController = mutantGO.GetComponent<EnemyController>();
        //flockAgent = mutantGO.GetComponent<FlockAgent>();

        playerGOs = GameObject.FindGameObjectsWithTag("Player");
        playerTransforms = new Transform[playerGOs.Length];
        for (int i = 0; i < playerGOs.Length; i++)
        {
            playerTransforms[i] = playerGOs[i].GetComponent<Transform>();
        }
        players = new Player[playerGOs.Length];
        for (int i = 0; i < playerGOs.Length; i++)
        {
            players[i] = playerGOs[i].GetComponent<Player>();
        }

    }

    //TODO: refactor this
    private Tuple<float, Transform> GetClosestPlayer()
    {
        float shortestDistance = float.MaxValue;
        Transform closestPlayerTransform = null;
        for (int i = 0; i < playerTransforms.Length; i++)
        {
            if (players[i].IsAlive()) // If the player is dead stop targeting.
            {
                float newDistance = Vector3.Distance(transform.position, playerTransforms[i].position);
                if (newDistance < shortestDistance)
                {
                    closestPlayerIndex = i;
                    closestPlayerTransform = playerTransforms[i];
                    shortestDistance = newDistance;
                }
            }

        }
        return Tuple.Create(shortestDistance, closestPlayerTransform);
    }

    void ComboStart()
    {
        mutant.cantMove = true;
    }

    //TODO REFACTOR JESSE: why are there 3 of these things?
    void Damage1()
    {
        closestPlayerPosition = GetClosestPlayer().Item2.position;

        float dist = Vector3.Distance(closestPlayerPosition, weaponTransform.position);

        if (dist <= 2.0f)
        {
            players[closestPlayerIndex].TakeDamage(mutant.abilityDamage);
        }

    }

    void Damage2()
    {
        closestPlayerPosition = GetClosestPlayer().Item2.position;

        float dist = Vector3.Distance(closestPlayerPosition, weaponTransform.position);

        if (dist <= 2.0f)
        {
            players[closestPlayerIndex].TakeDamage(mutant.abilityDamage);
        }

    }

    void Damage3()
    {
        closestPlayerPosition = GetClosestPlayer().Item2.position;

        float dist = Vector3.Distance(closestPlayerPosition, weaponTransform.position);

        if (dist <= 2.0f)
        {
            players[closestPlayerIndex].TakeDamage(mutant.abilityDamage);
        }

    }

    void ComboEnd()
    {
        mutant.cantMove = false;

    }
}