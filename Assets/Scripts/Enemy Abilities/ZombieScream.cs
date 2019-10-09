using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieScream : MonoBehaviour
{
	private Enemy zombie;

	public NavMeshAgent navMeshAgent;

	public Animator animator;
	void Start()
	{
		zombie = gameObject.GetComponent<Enemy>();
	}

	void screamStart()
	{
		zombie.cantMove = true;
	}

	void scream()
	{

	}

	void screamEnd()
	{
		zombie.cantMove = false;
	}

}
