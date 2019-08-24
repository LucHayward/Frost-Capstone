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


	public void OnTriggerEnter(Collider coll)
	{
		Instantiate(impactPrefab, transform.position, Quaternion.Inverse(transform.rotation));
		Destroy(gameObject);
	}

	//TODO implement object pooling for player projectiles 
	//public void OnDestroy()
	//{
	//	// Return object to projectile pool
	//}

}
