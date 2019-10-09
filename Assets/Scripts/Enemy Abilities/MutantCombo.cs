using System;
using UnityEngine;

public class MutantCombo : MonoBehaviour
{
	private Enemy mutant;
	public Transform weaponTransform;

	private Vector3 closestPlayerPosition;
	private Player closestPlayer;

	public Animator animator;

	public AudioSource attackClip;

	void Start()
	{
		mutant = gameObject.GetComponent<Enemy>();
	}

	void ComboStart()
	{
		mutant.cantMove = true;
	}

	/// <summary>
	/// Handles dealing damage to player 
	/// </summary>
	void Damage1()
	{
		Tuple<float, Transform, Player> tuple = GameManager.Get().GetClosestPlayer(weaponTransform);
		closestPlayer = tuple.Item3;
		closestPlayerPosition = tuple.Item2.position;

		float dist = Vector3.Distance(closestPlayerPosition, weaponTransform.position);

		if (dist <= 5.0f)
		{
			closestPlayer.TakeDamage(mutant.abilityDamage);
		}
	}

	void Damage2()
	{
		Damage1();
	}

	void Damage3()
	{
		Damage1();
	}

	void ComboEnd()
	{
		mutant.cantMove = false;
	}

	void PlayAudio()
	{
		attackClip.Play();
	}
}