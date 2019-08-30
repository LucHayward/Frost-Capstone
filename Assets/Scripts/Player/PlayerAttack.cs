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

	private LayerMask maskAll = -1;

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
		
		Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));

		RaycastHit hit;
		Physics.Raycast(ray, out hit, Mathf.Infinity, maskAll, QueryTriggerInteraction.Ignore);
		
		//TODO: Pool GameObjects for performance
		//TODO: Animate the spawning of a new object and fire the current one (or animate current respawn and instantiate new)
		GameObject firedGO = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity) as GameObject;

		// Hitting something we can aim at
		if (hit.collider != null && !hit.collider.CompareTag("Player"))
		{
			firedGO.transform.LookAt(hit.point);
		}
		else
		{ // Ray either didn't hit anything or it hit the player in which case can fire projectile along camera forward 
			firedGO.transform.rotation = cam.transform.rotation;
		}

		firedGO.GetComponent<Rigidbody>().AddForce(firedGO.transform.forward * 10, ForceMode.VelocityChange);
		firedGO.transform.rotation = Quaternion.AngleAxis(90, firedGO.transform.right);



		// DEBUG TO BE REMOVED
		if (hit.collider != null)
		{
			Debug.Log(hit.collider.ToString());
			Debug.DrawRay(projectileSpawnPoint.position, firedGO.transform.forward*3, Color.green, Vector3.Distance(projectileSpawnPoint.position, hit.point));
		}
		if (!firedGO.tag.Equals("Bullet")) Debug.LogError("Attack projectile has no bullet tag", firedGO);

		//Destroy(firedGO, 30);
	}

	private void CastHeal()
	{

	}

}
