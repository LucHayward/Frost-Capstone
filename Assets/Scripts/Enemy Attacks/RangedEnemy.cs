using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public int health;
    private int identificationNumber;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private FlockAgent flockAgent;

    private GameObject playerGO;
    private Player player;
    private Transform playerPos;
    public Animator animator;
    private Vector3 velocity;

    

    private Vector3 prevTransform;
    public float velocityMagnitude = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        prevTransform = transform.position;
        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        playerPos = playerGO.transform;

        animator.SetBool("isDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        velocity = ((transform.position - prevTransform) / Time.deltaTime);
        velocityMagnitude = velocity.magnitude;

        if (velocityMagnitude == 0)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
        }
        else
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", true);
        }
        


        prevTransform = transform.position;
        if (health < 1)
        {
            //animator.SetBool("isDead", true);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ID"></param>
    public void setIdentifier(int ID)
    {
        identificationNumber = ID;
    }

    public int getIdentifier()
    {
        return identificationNumber;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dmg"></param>
    public void takeDamage(int dmg)
    {
        health = health - dmg;
    }


    public void OnDisable()
    {
        enemyController.enabled = false;
        flockAgent.enabled = false;
    }

    public void OnEnable()
    {
        enemyController.enabled = true;
        flockAgent.enabled = true;
    }
}
