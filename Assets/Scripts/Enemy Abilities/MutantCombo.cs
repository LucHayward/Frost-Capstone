using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantCombo : MonoBehaviour
{

    private GameObject mutantGO;
    private Enemy mutant;
    public Transform weaponTransform;

    private GameObject playerGO;
    private Player player;

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


    void ComboStart()
    {
        mutant.cantMove = true;
    }

    void Damage1()
    {
        float dist = Vector3.Distance(playerTransform.position, weaponTransform.position);

        if (dist <= 2.0f)
        {
            player.TakeDamage(mutant.abilityDamage);
        }

    }

    void Damage2()
    {
        float dist = Vector3.Distance(playerTransform.position, weaponTransform.position);

        if (dist <= 2.0f)
        {
            player.TakeDamage(mutant.abilityDamage);
        }

    }

    void Damage3()
    {
        float dist = Vector3.Distance(playerTransform.position, weaponTransform.position);

        if (dist <= 2.0f)
        {
            player.TakeDamage(mutant.abilityDamage);
        }

    }

    void ComboEnd()
    {
        mutant.cantMove = false;

    }
}