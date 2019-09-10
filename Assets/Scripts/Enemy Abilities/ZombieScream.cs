using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieScream : MonoBehaviour
{

    private GameObject zombieGO;
    private Enemy zombie;

    private GameObject playerGO;
    private Player player;
    public NavMeshAgent navMeshAgent;

    public Animator animator;
    private EnemyController zombieController;
    private FlockAgent flockAgent;

    private Transform zombieTransform;
    private Transform playerTransform;

    
    // Start is called before the first frame update
    void Start()
    {
        zombieGO = gameObject;
        zombie = gameObject.GetComponent<Enemy>();
        zombieTransform = zombie.GetComponent<Transform>();

        zombieController = zombieGO.GetComponent<EnemyController>();
        flockAgent = zombieGO.GetComponent<FlockAgent>();

        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        playerTransform = player.GetComponent<Transform>();
    }

    void screamStart()
    {
        zombie.cantMove = true;
    }

    void scream()
    {
        //do something here. Attracts other enemies?
    }

    void screamEnd()
    {
        zombie.cantMove = false;
    }

}
