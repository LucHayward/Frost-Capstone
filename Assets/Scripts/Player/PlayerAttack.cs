using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the player attacks and abilities:
/// Input as well as the calculations, spawning and animation state updates are all managed here
/// </summary>
public class PlayerAttack : MonoBehaviour
{
	public float range = 25f;
	public Transform projectileSpawnPoint;
	public GameObject projectile;
	public Animator animator;
	public Camera cam;
	public Collider staffCollider;

	public AudioSource meleeAttackClip;
	public AudioSource attackClip;
	public AudioSource attackFailClip;
	public AudioSource attackStunCastClip;

	// used to notify PlayerMovement to face camera during melee
	[HideInInspector] public bool inMelee = false;

	private LayerMask maskAll = -1;
	private bool canAttack = true;
	private bool isPlayer1;

	private float lastStunCast = 0;

	private void Awake()
	{
		staffCollider.enabled = false;
	}

	public void setPlayerNum(int i)
	{
		isPlayer1 = i == 0;
	}

	/// <summary>
	/// Detects player input for all ability related inputs.
	/// </summary>
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

		if (Input.GetButtonDown(isPlayer1 ? "P1_Stun" : "P2_Stun"))
		{
			Stun();
		}

		if (Input.GetButtonDown(isPlayer1 ? "P1_Melee" : "P2_Melee"))
		{
			if (!canAttack || animator.GetCurrentAnimatorStateInfo(0).IsName("Melee Attack Stab"))
			{
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

	/// <summary>
	/// </summary>
	/// <returns>The dot product of the camera and the player on the horizontal plane as a float</returns>
	private float GetRelativeCameraOrientation()
	{
		Vector2 goHorizontal = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.z).normalized;
		Vector2 camHorizontal = new Vector2(cam.transform.forward.x, cam.transform.forward.z).normalized;

		float crossProd = Vector2.Dot(goHorizontal, camHorizontal);
		return crossProd;
	}

	/// <summary>
	/// </summary>
	/// <returns>Whether the players camera is facing backwards by more than a 45 degree rear angle.</returns>
	private bool CameraFacingBackwards()
	{
		return GetRelativeCameraOrientation() < -0.7f;
	}

	/// <summary>
	/// Handles the main shoot attack for the player.
	/// Ensures the player is not currently in the attack animation (designed to reduce attack-spam)
	/// Otherwise calculates the correct heading for the projectile based on the screenspace projection in the world
	/// Updates the animation state and sound effects
	/// </summary>
	/// <param name="dmg"> how damaging the projectile should be</param>
	private void Shoot(int dmg)
	{
		if (!canAttack || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || CameraFacingBackwards())
		{
			attackFailClip.Play();
			return;
		}
		canAttack = false;
		animator.SetTrigger("attack");
		attackClip.Play();

		bool multiplayer = GameManager.Get().players.Length > 1;
		Ray ray;
		if (multiplayer)
		{
			ray = cam.ScreenPointToRay(new Vector3((Screen.width / 4) + (isPlayer1 ? 0 : Screen.width / 2), Screen.height / 2, 0));
		}
		else
		{
			ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
		}

		RaycastHit hit;
		Physics.Raycast(ray, out hit, Mathf.Infinity, maskAll, QueryTriggerInteraction.Ignore);

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
	}

	/// <summary>
	/// Activates the players stun ability, stunning all the enemies for a variable period depending on how many stacks the enemy has
	/// on it.
	/// </summary>
	private void Stun()
	{
		if (!canAttack || animator.GetCurrentAnimatorStateInfo(0).IsName("Cast Stun") || Time.time - lastStunCast < 8)
		{
			attackFailClip.Play();
			return;
		}
		canAttack = false;
		attackStunCastClip.Play();
		lastStunCast = Time.time;

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

		animator.SetTrigger("meleeAttack");
		yield return new WaitForSecondsRealtime(1.2f);
		staffCollider.enabled = false;
		inMelee = false;
		ResetCanAttack();
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
