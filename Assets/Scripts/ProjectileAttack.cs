using UnityEngine;
using System.Collections;
using System.Linq;

public class ProjectileAttack : MonoBehaviour
{
	public int damage;
	public int objectLife;

	[TagSelector]
	public string [] damageableTags;

	public GameObject impactPrefab;

	private ParticleSystem ps;
	//private AudioSource audioSource;

    private bool isRanged = true;
	public void Start()
	{
		ps = gameObject.GetComponentInChildren<ParticleSystem>();
		//audioSource = gameObject.GetComponent<AudioSource>();
		Destroy(gameObject, objectLife); // TODO Do we need this>?
	}

	/// <summary>
	/// On colliding with another collider, check whether the object is able to be damaged by this projectile and deal damage.
	/// Spawns an impact prefab and destroys this projectile
	/// </summary>
	/// <param name="other"> collision representing this interaction</param>
	public void OnCollisionEnter(Collision other)
	{
		if (damageableTags.Contains(other.gameObject.tag))
		{
			if (other.gameObject.CompareTag("Enemy"))
			{
				Enemy enemy = other.gameObject.GetComponent<Enemy>();
				enemy.TakeDamage(damage, isRanged);
                enemy.AddStack();
			}
            else if (other.gameObject.CompareTag("Player"))
            {
				Player player = other.gameObject.GetComponent<Player>();
				player.TakeDamage(damage);
			}
		}
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
