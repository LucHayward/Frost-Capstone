using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class EnemyAttack : MonoBehaviour
{
	public Transform enemyWeaponTransform;
	private Transform enemyTransform;

	public Animator animator;
	private Enemy enemy;

	private EnemyController enemyController;
	private FlockAgent flockAgent;
	public NavMeshAgent navMeshAgent;

	// Ranges
	public float range = 0.0f;
	public float abilityRange = 0.0f;

	// Cooldowns
	public float shotDelay = 0.0f;
	public float abilityCD = 0.0f;

	// Attack Timers
	private float lastAttackTime = 0.0f;
	private float lastAbilityTime = 0.0f;

	private float currentTime = 0.0f;

	// 0 jumpAttack, 1 taunt
	private int abilityCounter = 0;

	// used for boss combo
	private int attackCounter = 0;

	//Audio
	public AudioSource attackClip;

	private Vector3 closestPlayerPosition;
	private Player closestPlayer;


	void Start()
	{
		enemy = gameObject.GetComponent<Enemy>();
		enemyTransform = enemy.GetComponent<Transform>();

		enemyController = gameObject.GetComponent<EnemyController>();
		flockAgent = gameObject.GetComponent<FlockAgent>();

	}

	/// <summary>
	/// Gets the closest player and initiates attack on that player if they are able to be attacked this frame
	/// Includes any animation state updates needed
	/// </summary>
	void Update()
	{
		Tuple<float, Transform, Player> tuple = GameManager.Get().GetClosestPlayer(transform);
		closestPlayerPosition = tuple.Item2.position;
		closestPlayer = tuple.Item3;
		currentTime = Time.time;

		if (Vector3.Distance(closestPlayerPosition, enemyTransform.position) < range)
		{
			if (currentTime - lastAttackTime > shotDelay)
			{
				if (attackCounter != 4)
				{
					animator.SetTrigger("atk");
					enemy.inVulnerable = false;
					lastAttackTime = currentTime + shotDelay;
					attackCounter++;
				}

				// if 5th attack then combo
				else
				{
					animator.SetTrigger("combo");
					enemy.inVulnerable = false;
					lastAttackTime = currentTime + shotDelay;

					attackCounter = 0;
				}
			}
		}
		else if (Vector3.Distance(closestPlayerPosition, enemyTransform.position) < abilityRange)
		{


			if (currentTime - lastAbilityTime > abilityCD)
			{
				if (abilityCounter == 0)
				{
					animator.SetTrigger("ability");
					enemy.inVulnerable = false;
					lastAbilityTime = currentTime + abilityCD;
					abilityCounter++;
				}

				else
				{
					animator.SetTrigger("taunt1");
					lastAbilityTime = currentTime + abilityCD;
					abilityCounter = 0;
				}
			}
		}

	}

	void attackStart()
	{
		enemy.cantMove = true;
	}

	void PlayAudio()
	{
		attackClip.Play();
	}

	void attackDamage()
	{
		// Calculate the distance between the player and the enemy
		float dist = Vector3.Distance(closestPlayerPosition, enemyWeaponTransform.position);

		if (dist <= 3.0f && enemy.type != "Ranged")
		{
			closestPlayer.TakeDamage(enemy.damage);
		}
	}

	void attackEnd()
	{
		enemy.cantMove = false;
	}
}
