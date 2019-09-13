using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedAttack : MonoBehaviour
{
    public float range = 0.0f;
    public float abilityRange = 0.0f;
    //public Transform skull;
    public Transform projectileSpawnPoint;
    public GameObject projectile;
    public Animator animator;
    private Enemy enemy;

    private EnemyController enemyController;
    private FlockAgent flockAgent;
    public NavMeshAgent navMeshAgent;

    Vector3 shotPath;
    private GameObject playerGO;
    private Transform playerTransform;

    

    private float currentTime = 0.0f;
    private float lastShotTime = 0.0f;
    private float shotDelay = 2.0f;

    private float lastAbilityTime = 0.0f;
    public float abilityCD = 0.0f;

    //0 at start, 1 animation started, 2 animation complete
    public int shotState;
    
    
    //public SpawnManager spawnManager;

    public ParticleSystem attackParticles; //TODO: Add particles
    public AudioSource attackClip; //TODO: Add attack audio

    private void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGO.GetComponent<Transform>();

        enemyController = gameObject.GetComponent<EnemyController>();
        flockAgent = gameObject.GetComponent<FlockAgent>();

        //skull.gameObject.GetComponent<ParticleSystem>().Play();
    }

    void Update()
    {
        Vector3 shotVector = new Vector3(playerTransform.position.x, 1, playerTransform.position.z);
        shotPath = shotVector - projectileSpawnPoint.position;
        currentTime = Time.time;
        
        if (currentTime - lastAbilityTime > abilityCD)   
        {
            if (Vector3.Distance(playerTransform.position, projectileSpawnPoint.position) < abilityRange)
            {
                
                animator.SetTrigger("ability");
                lastAbilityTime = currentTime + abilityCD;


            }

        }

        else if (Vector3.Distance(playerTransform.position, projectileSpawnPoint.position) <= range)
        {
            if (currentTime - lastShotTime > shotDelay)
            {
                //Animation event calls shoot at end of attack animation
                animator.SetTrigger("atk");
                lastShotTime = currentTime + shotDelay;
            }
                
        }
        


    }

    private void shootStart()
    {
        enemy.cantMove = true;
        
        enemy.velocityMagnitude = 0;
    }


    private void shoot()
    {
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

    private void shootEnd()
    {
        enemy.cantMove = false;
        
        
    }




}
