using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    public float range = 25f;
    //public Transform skull;
    public Transform projectileSpawnPoint;
    public GameObject projectile;
    public Animator animator;
    private Enemy enemy;

    Vector3 shotPath;
    private GameObject playerGO;
    private Transform playerTransform;

    private float currentTime = 0.0f;
    private float lastShotTime = 0.0f;
    private float shotDelay = 2.0f;

    
    //public SpawnManager spawnManager;

    public ParticleSystem attackParticles; //TODO: Add particles
    public AudioSource attackClip; //TODO: Add attack audio

    private void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGO.GetComponent<Transform>();
        //skull.gameObject.GetComponent<ParticleSystem>().Play();
    }

    void Update()
    {
        shotPath = playerTransform.position - projectileSpawnPoint.position;
        currentTime = Time.time;

        if (Vector3.Distance(playerTransform.position, projectileSpawnPoint.position) <= range)
        {
            enemy.velocityMagnitude = 0;
            if (currentTime - lastShotTime > shotDelay)
            {
                //Animation event calls shoot at end of attack animation
                animator.SetTrigger("atk");
                lastShotTime = currentTime + shotDelay;
            }
                
        }
    }

    private void Shoot()
    {

        //RaycastHit hit;
        //if(Physics.Raycast(raycastSource.position, raycastSource.forward, out hit, range)){

        //}

        //TODO: Pool GameObjects for performance
        //TODO: Animate the spawning of a new object and fire the current one (or animate current respawn and instantiate new)
        
        GameObject firedGO = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity) as GameObject;


        firedGO.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(firedGO.transform.forward, shotPath, 100f, 100f));
        firedGO.GetComponent<Rigidbody>().AddForce(firedGO.transform.forward * 20, ForceMode.VelocityChange);
        //Debug.DrawRay(projectileSpawnPoint.position, firedGO.transform.forward * 3, Color.green, Vector3.Distance(projectileSpawnPoint.position, shotPath));


        if (!firedGO.tag.Equals("Bullet")) Debug.LogError("Attack projectile has no bullet tag", firedGO);
        Destroy(firedGO, 30);

        //Debug.Log("Fired");
        


    }

    


}
