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
        witch.cantMove = true;
    }

    void ability()
    {
        proj1 = Instantiate(projectile, spawnPoints[0].position, Quaternion.identity) as GameObject;
        proj2 = Instantiate(projectile, spawnPoints[1].position, Quaternion.identity) as GameObject;
        proj3 = Instantiate(projectile, spawnPoints[2].position, Quaternion.identity) as GameObject;
        proj4 = Instantiate(projectile, spawnPoints[3].position, Quaternion.identity) as GameObject;


    }


    void ability2()
    {
        Vector3 shotVector = new Vector3(playerTransform.position.x, 1, playerTransform.position.z);

        shotPath1 = shotVector - spawnPoints[0].position;
        shotPath2 = shotVector - spawnPoints[1].position;
        shotPath3 = shotVector - spawnPoints[2].position;
        shotPath4 = shotVector - spawnPoints[3].position;

        if (proj1 != null)
        {
            proj1.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj1.transform.forward, shotPath1, 100f, 100f));
        }
        if (proj2 != null)
        {
            proj2.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj2.transform.forward, shotPath2, 100f, 100f));
        }
        if (proj3 != null)
        {
            proj3.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj3.transform.forward, shotPath3, 100f, 100f));
        }
        if (proj4 != null)
        {
            proj4.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj4.transform.forward, shotPath4, 100f, 100f));
        }

        if (proj1 != null)
        {
            proj1.GetComponent<Rigidbody>().AddForce(proj1.transform.forward * 20, ForceMode.VelocityChange);
        }
        if (proj2 != null)
        {
            proj2.GetComponent<Rigidbody>().AddForce(proj2.transform.forward * 20, ForceMode.VelocityChange);
        }
        if (proj3 != null)
        {
            proj3.GetComponent<Rigidbody>().AddForce(proj3.transform.forward * 20, ForceMode.VelocityChange);
        }
        if (proj4 != null)
        {
            proj4.GetComponent<Rigidbody>().AddForce(proj4.transform.forward * 20, ForceMode.VelocityChange);
        }

    }

    void ability2End()
    {
        witch.cantMove = false;

        if(proj1 != null)
        {
            Destroy(proj1, 3);
        }
        if (proj1 != null)
        {
            Destroy(proj2, 3);
        }
        if (proj1 != null)
        {
            Destroy(proj3, 3);
        }
        if (proj1 != null)
        {
            Destroy(proj4, 3);
        }
        
    }

    public void witchDead()
    {
        Destroy(proj1);
        Destroy(proj2);
        Destroy(proj3);
        Destroy(proj4);

        Destroy(witchGO);

    }

    public void WitchStun()
    {
        Destroy(proj1);
        Destroy(proj2);
        Destroy(proj3);
        Destroy(proj4);
    }

}
