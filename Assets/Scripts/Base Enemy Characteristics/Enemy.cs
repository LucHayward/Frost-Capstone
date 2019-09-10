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
    public int abilityDamage;
    public int damage;
    public bool hasScreamed;
    public bool isStunned;
    public float velocityMagnitude;
    public bool cantMove;

    [SerializeField] private EnemyController enemyController = null; //Assigned in inspector
	[SerializeField] private FlockAgent flockAgent = null; //Assigned in inspector
	private GameObject playerGO;
	private Player player;
	private Transform playerPos;
    private Vector3 prevTransform;
    private int numberOfStacks;

    public bool isDead = false;
	
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
        if (health <= 0)
        {
            UpdateScore();

            //if melee: SetTrigger("meleeDeath")
            //animator.SetBool("isRanged", false);

            //animator.SetBool("isDead", true);
            //animator.SetTrigger("meleeDeath");
            isDead = true;

            if (gameObject.GetType().Equals("Witch"))
            {
                gameObject.GetComponent<WitchAbility>().witchDead();
            }
            Destroy(gameObject);

            
        }
    }


    public void SetIdentifier(int ID)
    {
        identificationNumber = ID;
    }

    public int GetIdentifier()
    {
        return identificationNumber;
    }

    /// <summary>
    /// Reduce health by set amount
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

    /// <summary>
    /// Adds another stack onto the enemy
    /// </summary>
    public void AddStack()
    {
        numberOfStacks++;
    }

    /// <summary>
    /// Accessor method for stack count
    /// </summary>
    /// <returns> the number of stacks the enemy currently has on it </returns>
    public int GetStackCount()
    {
        return numberOfStacks;
    }

    public void ResetStackCount()
    {
        numberOfStacks = 0;
    }

    /// <summary>
    /// Wrapper method to run the stun coroutine
    /// </summary>
    public void StunCoroutineWrapper()
    {
        StartCoroutine(StunRoutine());
    }

    /// <summary>
    /// Slows down the enemy for a particular period based on stack count
    /// </summary>
    private IEnumerator StunRoutine()
    {
        DisableMovement();
        isStunned = true;
        float stunTime = GetStackCount() * 0.5f;
		Debug.Log("Stun time " + stunTime + "s");
		// TODO: DEBUG change here
		// yield return new WaitForSecondsRealtime(stunTime);
        yield return new WaitForSecondsRealtime(10);
		isStunned = false;
        EnableMovement();
        ResetStackCount();
    }

    public void DisableMovement()
    {
        enemyController.StopMove();
        flockAgent.enabled = false;
        enemyController.enabled = false;
    }

    public void EnableMovement()
    {
        enemyController.ResumeMove();
        flockAgent.enabled = true;
        enemyController.enabled = true;
    }


    void StartDeath()
    {

        //cantMove = true;
    }

    void EndDeath()
    {
        // Add particle effect
        //Destroy(gameObject);
    }
    
}
