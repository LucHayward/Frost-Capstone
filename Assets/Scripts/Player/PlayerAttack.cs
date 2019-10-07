using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
	public float range = 25f;
	//public Transform skull;
	public Transform projectileSpawnPoint;
	public GameObject projectile;
	public Animator animator;
	public Camera cam;
    public Collider staffCollider; 
	//public SpawnManager spawnManager;

	//public ParticleSystem attackParticles; //TODO: Add particles
	//public AudioSource attackClip; //TODO: Add attack audio
	public AudioSource meleeAttackClip;
	public AudioSource attackClip;
	public AudioSource attackFailClip;
	public AudioSource attackStunCastClip;


	[HideInInspector]public bool inMelee = false; // used to notify PlayerMovement to face camera during melee

	private LayerMask maskAll = -1;
	private bool canAttack = true;
	private bool isPlayer1;

	private void Awake()
	{
        //skull.gameObject.GetComponent<ParticleSystem>().Play();
        staffCollider.enabled = false;
	}

	public void setPlayerNum(int i)
	{
		isPlayer1 = i == 1;
	}

	void Update()
	{
		if (Input.GetButtonDown(isPlayer1 ? "P1_Fire" : "P2_Fire"))
		{
            if (gameObject.GetComponent<Player>().isRed)
            {
                Shoot(4);
            }
            else
            {
                Shoot(2);
            }
		}

        if(Input.GetButtonDown(isPlayer1 ? "P1_Stun" : "P2_Stun"))
        {
            Stun();
        }

        if(Input.GetButtonDown(isPlayer1 ? "P1_Melee" : "P2_Melee"))
        {
			if (!canAttack || animator.GetCurrentAnimatorStateInfo(0).IsName("Melee Attack Stab"))
			{
				//TODO: Update with unique melee fail clip
				attackFailClip.Play();
				return;
			}
			meleeAttackClip.Play();
			StartCoroutine(MeleeAttack());
        }

        if (Input.GetButtonDown(isPlayer1 ? "P1_SpeedBoost" : "P2_SpeedBoost"))
		{
			// Speed
            gameObject.GetComponent<Player>().UseFrostEssence("Blue");
        }
        if (Input.GetButtonDown(isPlayer1 ? "P1_DamageBoost" : "P2_DamageBoost"))
		{
			// Damage
            gameObject.GetComponent<Player>().UseFrostEssence("Red");
        }
        if (Input.GetButtonDown(isPlayer1 ? "P1_Heal" : "P2_Heal"))
		{
			// Heal
            gameObject.GetComponent<Player>().UseFrostEssence("Green");
        }
	}

	private float GetRelativeCameraOrientation()
	{
		Vector2 goHorizontal = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.z).normalized;
		Vector2 camHorizontal = new Vector2(cam.transform.forward.x, cam.transform.forward.z).normalized;

		float crossProd = Vector2.Dot(goHorizontal, camHorizontal);
		//Debug.Log("CrossProd: " + crossProd);
		return crossProd;
	}

	private bool CameraFacingBackwards()
	{
		return GetRelativeCameraOrientation() < -0.7f;
	}

	private void Shoot(int dmg)
	{
		if (!canAttack || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || CameraFacingBackwards())
		{
			Debug.Log("Cannot shot backwards");
			attackFailClip.Play();
			return;
		}
		canAttack = false;
		animator.SetTrigger("attack");
		attackClip.Play();
		
		Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));

		RaycastHit hit;
		Physics.Raycast(ray, out hit, Mathf.Infinity, maskAll, QueryTriggerInteraction.Ignore);
		
		//TODO: Pool GameObjects for performance
		//TODO: Animate the spawning of a new object and fire the current one (or animate current respawn and instantiate new)
		GameObject firedGO = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity) as GameObject;

        firedGO.GetComponent<ProjectileAttack>().SetDamage(dmg);
        firedGO.GetComponent<ProjectileAttack>().setPlayerNumOriginator(isPlayer1 ? 0 : 1);

        // Hitting something we can aim at
        if (hit.collider != null && !hit.collider.CompareTag("Player"))
		{
			firedGO.transform.LookAt(hit.point);
		}
		else
		{ // Ray either didn't hit anything or it hit the player in which case can fire projectile along camera forward 
			firedGO.transform.rotation = cam.transform.rotation;
		}

		firedGO.GetComponent<Rigidbody>().AddForce(firedGO.transform.forward * 10, ForceMode.VelocityChange);
		firedGO.transform.rotation = Quaternion.AngleAxis(90, firedGO.transform.right);


		// DEBUG TO BE REMOVED
		if (!firedGO.tag.Equals("Bullet")) Debug.LogError("Attack projectile has no bullet tag", firedGO);

		//Destroy(firedGO, 30);
	}

    /// <summary>
    /// Activates the players stun ability, stunning all the enemies for a variable period depending on how many stacks the enemy has
    /// on it.
    /// </summary>
    private void Stun()
    {
		if (!canAttack || animator.GetCurrentAnimatorStateInfo(0).IsName("Cast Stun"))
		{
			// TODO: Find new fail clip
			attackFailClip.Play();
			return;
		}
		canAttack = false;
		attackStunCastClip.Play();

		animator.SetTrigger("castStun");
		foreach (EnemyManager meleeEnemy in GameManager.Get().meleeEnemies)
        {
            meleeEnemy.Stun();
        }

        foreach (EnemyManager rangedEnemy in GameManager.Get().rangedEnemies)
        {
            rangedEnemy.Stun();
        }

        foreach (EnemyManager bossEnemy in GameManager.Get().bossEnemies)
        {
            bossEnemy.Stun();
        }
    }

    /// <summary>
    /// Initiates a melee attack by the player
    /// </summary>
    private IEnumerator MeleeAttack()
    {
		inMelee = true;
		canAttack = false;
        staffCollider.enabled = true;
        Debug.Log("Melee Attack");
		animator.SetTrigger("meleeAttack");
		yield return new WaitForSecondsRealtime(1.2f); // TODO: remove magic number (melee attack animation length)
        staffCollider.enabled = false;
		inMelee = false;
		ResetCanAttack();
    }

	/// <summary>
	/// Initiates a heal ability by the player
	/// </summary>
	private void CastHeal()
	{

	}


    public void OnDisable()
    {
        if (cam != null)
        {
            cam.GetComponent<CameraController>().enabled = false;
        }
        
    }

    public void OnEnable()
    {
		if (cam != null)
		{
			cam.GetComponent<CameraController>().enabled = true;
		}
    }

	private void ResetCanAttack() { canAttack = true; }
}
