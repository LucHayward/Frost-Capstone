using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantTaunt : MonoBehaviour
{

    private GameObject mutantGO;
    private Enemy mutant;


    public Animator animator;


    private Transform mutantTransform;


    // Start is called before the first frame update
    void Start()
    {
        mutantGO = gameObject;
        mutant = gameObject.GetComponent<Enemy>();
        mutantTransform = mutant.GetComponent<Transform>();

    }


    void TauntStart()
    {
        mutant.cantMove = true;
        mutant.inVulnerable = true;
    }


    void TauntEnd()
    {
        mutant.cantMove = false;
        mutant.inVulnerable = false;

    }
}
