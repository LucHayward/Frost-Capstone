using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MutantAbility : MonoBehaviour
{

    private GameObject mutantGO;
    private Enemy mutant;
    public Transform weaponTransform;

    private GameObject[] playerGOs;
    private Player[] players;
    private Transform[] playerTransforms;
    private Vector3 closestPlayerPosition;
    private int closestPlayerIndex;
    public NavMeshAgent navMeshAgent;

    public Animator animator;

    //Audio
    public AudioSource roarAudio;
    public AudioSource landAudio;
    //private EnemyController mutantController;
    //private FlockAgent flockAgent;

    //private Transform mutantTransform;


    // Start is called before the first frame update
    void Start()
    {
        mutantGO = gameObject;
        mutant = gameObject.GetComponent<Enemy>();
        //mutantTransform = mutant.GetComponent<Transform>();

        //mutantController = mutantGO.GetComponent<EnemyController>();
        //flockAgent = mutantGO.GetComponent<FlockAgent>();

        //TODO: Refactor this in every single enemy class
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

    //TODO: Refactor into method in enemy controller or something
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

    void jumpAttackStart()
    {
        mutant.cantMove = true;
        roarAudio.Play();
    }

    void jumpAttackMove()
    {
        mutant.cantMove = false;

    }

    void PlayLandAudio()
    {
        landAudio.Play();
    }

    //TODO REFACTOR JESSE: This is the same as in MutantCombo
    void jumpAttackDamage()
    {
        roarAudio.Pause();
        closestPlayerPosition = GetClosestPlayer().Item2.position;

        float dist = Vector3.Distance(closestPlayerPosition, weaponTransform.position);
        
        if (dist <= 5.0f)
        {
            players[closestPlayerIndex].TakeDamage(mutant.abilityDamage);
        }
        
        
    }
    
    void jumpAttackLand()
    {
        mutant.cantMove = true;

    }

    void jumpAttackEnd()
    {
        
        mutant.cantMove = false;
        landAudio.Pause();

    }
}
