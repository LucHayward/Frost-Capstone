using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    public int health;
    private int identificationNumber;
    public Animator animator;
    private Vector3 velocity;
    public string type;
    public int abilityDamage = 0;
    public int damage = 0;
    public bool hasScreamed = false;
    public float velocityMagnitude = 0.0f;
    //public bool busyAttacking = false;
    //public bool busyAbility = false;
    //public bool busyShooting = false;

    public bool cantMove = false;

    [SerializeField] private EnemyController enemyController;
    [SerializeField] private FlockAgent flockAgent;
	private GameObject playerGO;
	private Player player;
	private Transform playerPos;
    private Vector3 prevTransform;

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

        if(velocityMagnitude == 0.1f)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);
        }
        else if (velocityMagnitude == 0)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);
        }
        else if (velocityMagnitude < 2f)
        {

            //removed walking animation for now
            animator.SetBool("Idle", false);
            animator.SetBool("Run", true);
            animator.SetBool("Walk", false);
        }
        else if(velocityMagnitude > 2f)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Run", true);
            animator.SetBool("Walk", false);
        }
        
        prevTransform = transform.position;
        if (health < 1)
        {
            UpdateScore();
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
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        health -= damage;
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

    private void UpdateScore()
    {
        if (type == "Boss")
            GameManager.Get().UpdateScore(5);
        else if (type == "Melee")
            GameManager.Get().UpdateScore(3);
        else if (type == "Ranged")
            GameManager.Get().UpdateScore(4);
    }

    //add stack method - called from within projectile attack (line 38)
    
}
