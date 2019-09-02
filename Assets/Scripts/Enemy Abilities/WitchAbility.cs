using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WitchAbility : MonoBehaviour
{

    private GameObject witchGO;
    private Enemy witch;
    public GameObject projectile;

    private GameObject playerGO;
    private Player player;
    public NavMeshAgent navMeshAgent;

    public Animator animator;
    private EnemyController witchController;
    private FlockAgent flockAgent;

    private Transform witchTransform;
    private Transform playerTransform;

    private Vector3 shotPath1;
    private Vector3 shotPath2;
    private Vector3 shotPath3;
    private Vector3 shotPath4;


    public Transform [] spawnPoints;

    private GameObject proj1;
    private GameObject proj2;
    private GameObject proj3;
    private GameObject proj4;

    private GameObject temp1;
    private GameObject temp2;
    private GameObject temp3;
    private GameObject temp4;




    // Start is called before the first frame update
    void Start()
    {
        
        witchGO = gameObject;
        witch = gameObject.GetComponent<Enemy>();
        witchTransform = witch.GetComponent<Transform>();

        witchController = witchGO.GetComponent<EnemyController>();
        flockAgent = witchGO.GetComponent<FlockAgent>();

        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        playerTransform = player.GetComponent<Transform>();
    }



    void abilityStart()
    {
        witchController.enabled = false;
        flockAgent.enabled = false;
        navMeshAgent.enabled = false;
    }

    void ability()
    {
        temp1 = Instantiate(projectile, spawnPoints[0].position, Quaternion.identity) as GameObject;
        temp2 = Instantiate(projectile, spawnPoints[1].position, Quaternion.identity) as GameObject;
        temp3 = Instantiate(projectile, spawnPoints[2].position, Quaternion.identity) as GameObject;
        temp4 = Instantiate(projectile, spawnPoints[3].position, Quaternion.identity) as GameObject;

        


        //temp1.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(temp1.transform.forward, shotPath1, 100f, 100f));
        //temp2.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(temp2.transform.forward, shotPath2, 100f, 100f));
        //temp3.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(temp3.transform.forward, shotPath3, 100f, 100f));
        //temp4.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(temp4.transform.forward, shotPath4, 100f, 100f));



    }

    void abilityEnd()
    {
        //witchController.enabled = true;
        //flockAgent.enabled = true;
        //navMeshAgent.enabled = true;

        animator.SetTrigger("ability2");
    }

    void ability2Start()
    {
        witchController.enabled = false;
        flockAgent.enabled = false;
        navMeshAgent.enabled = false;


        Destroy(temp1);
        temp1 = Instantiate(projectile, spawnPoints[0].position, Quaternion.identity) as GameObject;
        Destroy(temp2);
        temp2 = Instantiate(projectile, spawnPoints[1].position, Quaternion.identity) as GameObject;
        Destroy(temp3);
        temp3 = Instantiate(projectile, spawnPoints[2].position, Quaternion.identity) as GameObject;
        Destroy(temp4);
        temp4 = Instantiate(projectile, spawnPoints[3].position, Quaternion.identity) as GameObject;


        //temp1.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(temp1.transform.forward, shotPath1, 100f, 100f));
        //temp2.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(temp2.transform.forward, shotPath2, 100f, 100f));
        //temp3.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(temp3.transform.forward, shotPath3, 100f, 100f));
        //temp4.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(temp4.transform.forward, shotPath4, 100f, 100f));


    }


    void ability2()
    {

        Destroy(temp1);
        proj1 = Instantiate(projectile, spawnPoints[0].position, Quaternion.identity) as GameObject;

        Destroy(temp2);
        proj2 = Instantiate(projectile, spawnPoints[1].position, Quaternion.identity) as GameObject;

        Destroy(temp3);
        proj3 = Instantiate(projectile, spawnPoints[2].position, Quaternion.identity) as GameObject;

        Destroy(temp4);
        proj4 = Instantiate(projectile, spawnPoints[3].position, Quaternion.identity) as GameObject;

        shotPath1 = playerTransform.position - spawnPoints[0].position;
        shotPath2 = playerTransform.position - spawnPoints[1].position;
        shotPath3 = playerTransform.position - spawnPoints[2].position;
        shotPath4 = playerTransform.position - spawnPoints[3].position;

        proj1.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj1.transform.forward, shotPath1, 100f, 100f));
        proj2.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj2.transform.forward, shotPath2, 100f, 100f));
        proj3.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj3.transform.forward, shotPath3, 100f, 100f));
        proj4.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj4.transform.forward, shotPath4, 100f, 100f));


        proj1.GetComponent<Rigidbody>().AddForce(proj1.transform.forward * 20, ForceMode.VelocityChange);
        proj2.GetComponent<Rigidbody>().AddForce(proj2.transform.forward * 20, ForceMode.VelocityChange);
        proj3.GetComponent<Rigidbody>().AddForce(proj3.transform.forward * 20, ForceMode.VelocityChange);
        proj4.GetComponent<Rigidbody>().AddForce(proj4.transform.forward * 20, ForceMode.VelocityChange);

        Destroy(proj1, 30);
        Destroy(proj2, 30);
        Destroy(proj3, 30);
        Destroy(proj4, 30);
        
    }

    void ability2End()
    {
        witchController.enabled = true;
        flockAgent.enabled = true;
        navMeshAgent.enabled = true;
    }
}
