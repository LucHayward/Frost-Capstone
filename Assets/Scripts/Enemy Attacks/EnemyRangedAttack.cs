using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedAttack : MonoBehaviour
{
	public float range = 0.0f;
	public float abilityRange = 0.0f;

	public Transform projectileSpawnPoint;
	public GameObject projectile;
	public Animator animator;
	private Enemy enemy;

	public NavMeshAgent navMeshAgent;

	Vector3 shotPath;

	private float currentTime = 0.0f;
	private float lastShotTime = 0.0f;
	private float shotDelay = 2.0f;

	private float lastAbilityTime = 0.0f;
	public float abilityCD = 0.0f;

	//0 at start, 1 animation started, 2 animation complete
	public int shotState;

	public ParticleSystem attackParticles; //TODO: Add particles
	public AudioSource attackClip; //TODO: Add attack audio

	private void Start()
	{
		enemy = gameObject.GetComponent<Enemy>();
	}

	/// <summary>
	/// Handles the timing of the enemy ranged attack rate.
	/// Responsible for updating the animation states.
	/// </summary>
	void Update()
	{
		Vector3 closestPlayerPosition = GameManager.Get().GetClosestPlayer(transform).Item2.position;
		Vector3 shotVector = new Vector3(closestPlayerPosition.x, 1, closestPlayerPosition.z);
		shotPath = shotVector - projectileSpawnPoint.position;
		currentTime = Time.time;

		if (currentTime - lastAbilityTime > abilityCD)
		{
			if (Vector3.Distance(closestPlayerPosition, projectileSpawnPoint.position) < abilityRange)
			{
				animator.SetTrigger("ability");
				lastAbilityTime = currentTime + abilityCD;
			}
		}

		else if (Vector3.Distance(closestPlayerPosition, projectileSpawnPoint.position) <= range)
		{
			if (currentTime - lastShotTime > shotDelay)
			{
				//Animation event calls shoot at end of attack animation
				animator.SetTrigger("atk");
				lastShotTime = currentTime + shotDelay;
			}
		}
	}

	private void shootStart()
	{
		enemy.cantMove = true;

		enemy.velocityMagnitude = 0;
	}

	/// <summary>
	/// Handles spawning and firing a projectile
	/// </summary>
	private void shoot()
	{
		GameObject firedGO = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity) as GameObject;

		firedGO.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(firedGO.transform.forward, shotPath, 100f, 100f));
		attackClip.Play();
		firedGO.GetComponent<Rigidbody>().AddForce(firedGO.transform.forward * 20, ForceMode.VelocityChange);

		Destroy(firedGO, 30);
	}

	private void shootEnd()
	{
		enemy.cantMove = false;
	}
}
