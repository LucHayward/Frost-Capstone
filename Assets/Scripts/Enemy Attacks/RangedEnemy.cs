using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
	public int health;
	private int identificationNumber;
	[SerializeField] private EnemyController enemyController = null; //Assigned in inspector
	[SerializeField] private FlockAgent flockAgent = null; //Assigned in inspector

	public Animator animator;
	private Vector3 velocity;



	private Vector3 prevTransform;
	public float velocityMagnitude = 0.0f;

	void Start()
	{
		prevTransform = transform.position;

		animator.SetBool("isDead", false);
	}

	/// <summary>
	/// Update the velocity and animation state each frame
	/// </summary>
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

	public void setIdentifier(int ID)
	{
		identificationNumber = ID;
	}

	public int getIdentifier()
	{
		return identificationNumber;
	}

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
