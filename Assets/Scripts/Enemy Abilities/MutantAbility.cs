using System.Collections;
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


    void jumpAttackStart()
    {
        mutant.cantMove = true;
    }

    void jumpAttackMove()
    {
        mutant.cantMove = false;

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
        mutant.cantMove = true;

    }

    void jumpAttackEnd()
    {
        mutant.cantMove = false;

    }
}
