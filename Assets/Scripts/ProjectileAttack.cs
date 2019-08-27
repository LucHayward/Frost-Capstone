using UnityEngine;
using System.Collections;

public class ProjectileAttack : MonoBehaviour
{
	public int damage;
	public int objectLife;
	public bool canFriendlyFire;

	public GameObject impactPrefab;

	private ParticleSystem ps;
	private AudioSource audioSource;



	public void Start()
	{
		ps = gameObject.GetComponentInChildren<ParticleSystem>();
		audioSource = gameObject.GetComponent<AudioSource>();
		Destroy(gameObject, objectLife); // TODO Do we need this>?
	}


	public void OnCollisionEnter(Collision other)
	{
		// Instantiate the impact effect at the projectile transform pointing in the direction of the contact normals
		Vector3 contactNormal = other.GetContact(0).normal;
		Quaternion rotation = Quaternion.LookRotation(contactNormal);
		Instantiate(impactPrefab, transform.position, rotation);
		Destroy(gameObject);
	}

	//TODO implement object pooling for player projectiles 
	//public void OnDestroy()
	//{
	//	// Return object to projectile pool
	//}

}
