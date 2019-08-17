using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossShooter : MonoBehaviour
{
    public GameObject bullet;
    private Transform bossPos;
    private Boss boss;


    public float coolDown;
    public int abLevel;
    public float shootTime = 0;
    public float waitTime;
    public float abStart;
    public float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        bossPos = GameObject.FindGameObjectWithTag("Boss").transform; ;
        
        
    }

    // Update is called once per frame
    void Update()
    {

        currentTime = Time.time;
        

        if (Time.time > shootTime)
        {
            shootTime = Time.time + coolDown;
            Shoot();
        }
        
    }

    void Shoot()
    {
        for(int i = 0; i< SceneManager.GetActiveScene().buildIndex+1; i++)
        {
            Instantiate(bullet, bossPos.position, Quaternion.identity);
        }
        
    }
}
