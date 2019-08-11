using UnityEngine;
using System.Collections;

public class ProjectileAttack : MonoBehaviour
{
	public int damage;
	public int duration;
	public bool canFriendlyFire;

	public ParticleSystem ps;
	public AudioClip impactAudio;

	public void OnCollisionEnter(Collision coll)
	{
		if (ps != null)
		{
			//TODO: check this is correct style
			//ParticleSystem tempPs = Instantiate(ps);
			//Destroy(tempPs, tempPs.main.duration);
			ps.Play();
			Destroy(gameObject, ps.main.duration);
		}
		else
		{
			Debug.Log("Destroyed projectile", gameObject);
		}
	}

}
