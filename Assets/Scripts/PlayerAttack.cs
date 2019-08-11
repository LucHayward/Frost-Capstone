using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	public float range = 25f;
	//public Transform skull;
	public Transform projectileSpawnPoint;
	public GameObject projectile;
	public Animator animator;
	public Camera cam;

	//public SpawnManager spawnManager;

	public ParticleSystem attackParticles; //TODO: Add particles
	public AudioSource attackClip; //TODO: Add attack audio

	private void Awake()
	{
		//skull.gameObject.GetComponent<ParticleSystem>().Play();
	}

	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		animator.SetTrigger("atk");
		//RaycastHit hit;
		//if(Physics.Raycast(raycastSource.position, raycastSource.forward, out hit, range)){

		//}

		//TODO: Pool GameObjects for performance
		//TODO: Animate the spawning of a new object and fire the current one (or animate current respawn and instantiate new)
		GameObject firedGO = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity) as GameObject;

		Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
		//Debug.DrawRay(ray.origin, ray.direction*3, Color.green, 10);
		RaycastHit hit;
		Physics.Raycast(ray, out hit);
		firedGO.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(firedGO.transform.forward, hit.point - firedGO.transform.position, 100f, 100f));
		firedGO.GetComponent<Rigidbody>().AddForce(firedGO.transform.forward * 10, ForceMode.VelocityChange);
		Debug.DrawRay(projectileSpawnPoint.position, firedGO.transform.forward*3, Color.green, Vector3.Distance(projectileSpawnPoint.position, hit.point));


		if (!firedGO.tag.Equals("Bullet")) Debug.LogError("Attack projectile has no bullet tag", firedGO);
		Destroy(firedGO, 30);
		animator.ResetTrigger("atk");
	}


}
