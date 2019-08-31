using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyManager
{
    [HideInInspector] public GameObject instanceOfEnemy;
    public Transform spawnPoint;
    private Collider[] colliders;
    private Enemy enemyScript;
    private EnemyController enemyController;
    private FlockAgent flockAgent;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ID"></param>
    public void Setup(int ID)
    {
        enemyController = instanceOfEnemy.GetComponent<EnemyController>();
        enemyScript = instanceOfEnemy.GetComponent<Enemy>();
        flockAgent = instanceOfEnemy.GetComponent<FlockAgent>();
        enemyScript.setIdentifier(ID);
    }

    public int getID()
    {
        return enemyScript.getIdentifier();
    }

    public void EnableMovement()
    {
        enemyScript.enabled = true;
    }

    public void DisableMovement()
    {
        enemyScript.enabled = false;
    }

    public void CalculateSpawnPoint()
    {
        colliders = Physics.OverlapSphere(spawnPoint.position, 10);
        bool spawnHere = false;
        bool complete = false;
        Vector3 pointOfSpawn = Vector3.zero;
        while (complete != true)
        {
            Vector2 spawnArea = Random.insideUnitCircle * 10;
            pointOfSpawn = new Vector3(spawnPoint.position.x + spawnArea.x, 1, spawnPoint.position.z + spawnArea.y);
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].bounds.Contains(pointOfSpawn))
                {
                    spawnHere = false;
                }
                else
                {
                    spawnHere = true;
                }
            }
            if(spawnHere == true)
            {
                complete = true;
            }
        }
        spawnPoint.position = pointOfSpawn;  
    }

    public FlockAgent GetFlockAgent()
    {
        return flockAgent;
    }
}
