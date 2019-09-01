using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
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

    public bool hasScreamed = false;

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
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);
        }
        else if (velocityMagnitude < 2f)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Run", false);
            animator.SetBool("Walk", true);
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

    public void Scream()
    {
        //agent.isStopped = true;
        //flockAgent.enabled = false;
        animator.SetTrigger("scream");
        

    }
}
