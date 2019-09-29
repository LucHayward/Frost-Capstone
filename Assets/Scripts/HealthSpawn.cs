using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawn : MonoBehaviour
{
    private float currentTime = 0.0f;
    public float healthDropCD = 0.0f;
    private float timeUsed = 0.0f;
    private bool isHealth = false;

    public Transform spawnPoint;

    public GameObject healthDropGO;
    // Start is called before the first frame update
    void Start()
    {
        SpawnHealth();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;

        if(currentTime - timeUsed > healthDropCD)
        {
            if (!isHealth)
            {
                SpawnHealth();
                isHealth = true;
            }
        }

        
    }

    public void HealthUsed()
    {
        timeUsed = currentTime + healthDropCD;
        isHealth = false;
    }

    public void SpawnHealth()
    {
        GameObject healthDrop = Instantiate(healthDropGO, spawnPoint.position, Quaternion.identity) as GameObject;
        isHealth = true;
    }
}
