using System;
using UnityEngine;
using UnityEngine.AI;

public class MutantAbility : MonoBehaviour
{
	private Enemy mutant;
	public Transform weaponTransform;

	private Vector3 closestPlayerPosition;
	private Player closestPlayer;

	public NavMeshAgent navMeshAgent;

	public Animator animator;

	public AudioSource roarAudio;
	public AudioSource landAudio;

	void Start()
	{
		mutant = gameObject.GetComponent<Enemy>();
	}

	void jumpAttackStart()
	{
		mutant.cantMove = true;
		roarAudio.Play();
	}

	void jumpAttackMove()
	{
		mutant.cantMove = false;
	}

	void PlayLandAudio()
	{
		landAudio.Play();
	}

	/// <summary>
	/// Handles dealing damage to player 
	/// </summary>
	void jumpAttackDamage()
	{
		roarAudio.Pause();
		Tuple<float, Transform, Player> tuple = GameManager.Get().GetClosestPlayer(weaponTransform);
		closestPlayer = tuple.Item3;
		closestPlayerPosition = tuple.Item2.position;

		float dist = Vector3.Distance(closestPlayerPosition, weaponTransform.position);

		if (dist <= 5.0f)
		{
			closestPlayer.TakeDamage(mutant.abilityDamage);
		}
	}

	void jumpAttackLand()
	{
		mutant.cantMove = true;
	}

	void jumpAttackEnd()
	{
		mutant.cantMove = false;
		landAudio.Pause();
	}
}
