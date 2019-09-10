using UnityEngine;
using System.Collections.Generic;
public class PlayerAttack : MonoBehaviour
{
	public float range = 25f;
	//public Transform skull;
	public Transform projectileSpawnPoint;
	public GameObject projectile;
	public Animator animator;
	public Camera cam;

	//public SpawnManager spawnManager;

	//public ParticleSystem attackParticles; //TODO: Add particles
	//public AudioSource attackClip; //TODO: Add attack audio
	public AudioSource attackFailClip;

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

        if(Input.GetButtonDown("Fire2"))
        {
            Stun();
        }
	}

	private float GetRelativeCameraOrientation()
	{
		Vector2 goHorizontal = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.z).normalized;
		Vector2 camHorizontal = new Vector2(cam.transform.forward.x, cam.transform.forward.z).normalized;

		float crossProd = Vector2.Dot(goHorizontal, camHorizontal);
		Debug.Log("CrossProd: " + crossProd);
		return crossProd;
	}

	private bool CameraFacingBackwards()
	{
		return GetRelativeCameraOrientation() < -0.7f;
	}

	private void Shoot()
	{
		if (CameraFacingBackwards())
		{
			Debug.Log("Cannot shot backwards");
			attackFailClip.Play();
			return;
		}
		animator.SetTrigger("attack");
		
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
		if (!firedGO.tag.Equals("Bullet")) Debug.LogError("Attack projectile has no bullet tag", firedGO);

		//Destroy(firedGO, 30);
	}

    /// <summary>
    /// The players stun ability that stuns all the enemies for a variable period depending on how many stacks the enemy has
    /// on it.
    /// </summary>
    private void Stun()
    {
        //animator.SetTrigger("stun");
        foreach (EnemyManager meleeEnemy in GameManager.Get().meleeEnemies)
        {
            meleeEnemy.Stun();
        }

        foreach (EnemyManager rangedEnemy in GameManager.Get().rangedEnemies)
        {
            rangedEnemy.Stun();
        }

        foreach (EnemyManager bossEnemy in GameManager.Get().bossEnemies)
        {
            bossEnemy.Stun();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void MeleeAttack()
    {

    }

	private void CastHeal()
	{

	}

    public void OnDisable()
    {
        cam.GetComponent<CameraController>().enabled = false;
    }

    public void OnEnable()
    {
        cam.GetComponent<CameraController>().enabled = true;
    }

}
