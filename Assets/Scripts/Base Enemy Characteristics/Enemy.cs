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
    public float velocityMagnitude;

	public string type;
    public int abilityDamage;
    public int damage;
    public bool hasScreamed;
    public bool isStunned;
   
    public bool cantMove;
    public bool isDead;

    //types of frost essence
    public GameObject blueFE;
    public GameObject redFE;
    public GameObject greenFE;

    public Transform redFEtransform;
    public Transform blueFEtransform;
    public Transform greenFEtransform;

    private bool rangedDeath;

    [SerializeField] private EnemyController enemyController = null; //Assigned in inspector
	[SerializeField] private FlockAgent flockAgent = null; //Assigned in inspector
	private GameObject playerGO;
	private Player player;
	private Transform playerPos;

    private Vector3 prevTransform;
    private int numberOfStacks;

    //public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        prevTransform = transform.position;
		playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        playerPos = playerGO.transform;

        //animator.SetBool("isDead", false);
        isDead = false;

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
        if (health <= 0 && isDead == false)
        {
            cantMove = true;
            UpdateScore();

            //if melee: SetTrigger("meleeDeath")
            //animator.SetBool("isRanged", false);
            //isDead = true;
            if (rangedDeath && type == "Melee")
            {
                animator.SetTrigger("rangedDeath");
            }
            else
            {
                animator.SetTrigger("meleeDeath");
            }
            isDead = true;
            GameManager.Get().RemovedDeadEnemy(identificationNumber, type);
        }

        if(GameManager.Get().gameOver == true)
        {
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
    public void TakeDamage(int damage, bool isRangedAtk)
    {
        health -= damage;
        rangedDeath = isRangedAtk;
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
        //animator.SetTrigger("stunned");
    }

    /// <summary>
    /// Slows down the enemy for a particular period based on stack count
    /// </summary>
    private IEnumerator StunRoutine()
    {
        //DisableMovement();
        if (GetStackCount() > 0)
        {
            if(type == "Ranged")
            {
                gameObject.GetComponent<WitchAbility>().WitchStun();
            }
            //Stops movement in EnemyController Update()
            isStunned = true;
            //Start stun animation
            animator.SetBool("isStunned", true);
            //DisableMovement();
            float stunTime = GetStackCount() * 2.0f;
            Debug.Log("Stun time " + stunTime + "s");
            yield return new WaitForSecondsRealtime(stunTime);

            //End stun animation
            animator.SetBool("isStunned", false);
            //yield return new WaitForSecondsRealtime(10);
            isStunned = false;
            cantMove = false;
            ResetStackCount();
        }

        //isStunned = false;
        //cantMove = false;
    }

    void StartDeath()
    {
        
        cantMove = true;
    }

    void EndDeath()
    {
        DropEssence();
        if (type == "Ranged")
        {
            gameObject.GetComponent<WitchAbility>().witchDead();

        }
        else { Destroy(gameObject); }

        

    }

    void DropEssence()
    {
        if(type == "Ranged")
        {
            //random number to determine drop type. Higher range for weaker enemies (lower chance of drop)
            var dropType = Random.Range(0, 15);


            //random number to determine drop amount. Lower range for weaker enemies (lower amount dropped)
            var dropAmount = Random.Range(10, 51);

            if(dropType < 3)
            {
                GameObject blueFrostEssence = Instantiate(blueFE, gameObject.transform.position, Quaternion.identity) as GameObject;

                blueFrostEssence.GetComponent<FrostEssence>().setAmount(dropAmount);
            }
            else if (dropType < 6)
            {
                GameObject redFrostEssence = Instantiate(redFE, gameObject.transform.position, Quaternion.identity) as GameObject;
                redFrostEssence.GetComponent<FrostEssence>().setAmount(dropAmount);
            }
            else if(dropType < 9)
            {
                GameObject greenFrostEssence = Instantiate(greenFE, gameObject.transform.position, Quaternion.identity) as GameObject;
                greenFrostEssence.GetComponent<FrostEssence>().setAmount(dropAmount);
            }
        }

        else if (type == "Melee")
        {
            //random number to determine drop type. Higher range for weaker enemies (lower chance of drop)
            var dropType = Random.Range(0, 20);

            //random number to determine drop amount. Lower range for weaker enemies (lower amount dropped)
            var dropAmount = Random.Range(5, 31);

            if (dropType < 3)
            {
                GameObject blueFrostEssence = Instantiate(blueFE, gameObject.transform.position, Quaternion.identity) as GameObject;

                blueFrostEssence.GetComponent<FrostEssence>().setAmount(dropAmount);
            }
            else if (dropType < 6)
            {
                GameObject redFrostEssence = Instantiate(redFE, gameObject.transform.position, Quaternion.identity) as GameObject;
                redFrostEssence.GetComponent<FrostEssence>().setAmount(dropAmount);
            }
            else if (dropType < 9)
            {
                GameObject greenFrostEssence = Instantiate(greenFE, gameObject.transform.position, Quaternion.identity) as GameObject;
                greenFrostEssence.GetComponent<FrostEssence>().setAmount(dropAmount);
            }
        }

        else if (type == "Boss")
        {
            //random number to determine drop amount. Lower range for weaker enemies (lower amount dropped)
            var redAmount = Random.Range(50, 91);
            var blueAmount = Random.Range(50, 91);
            var greenAmount = Random.Range(50, 91);


            GameObject blueFrostEssence = Instantiate(blueFE, blueFEtransform.position, Quaternion.identity) as GameObject;
            blueFrostEssence.GetComponent<FrostEssence>().setAmount(blueAmount);
            
            
            GameObject redFrostEssence = Instantiate(redFE, redFEtransform.position, Quaternion.identity) as GameObject;
            redFrostEssence.GetComponent<FrostEssence>().setAmount(redAmount);
            
            
            GameObject greenFrostEssence = Instantiate(greenFE, greenFEtransform.position, Quaternion.identity) as GameObject;
            greenFrostEssence.GetComponent<FrostEssence>().setAmount(greenAmount);
            
        }
    }


}
