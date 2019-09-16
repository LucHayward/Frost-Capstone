using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyManager
{
    [HideInInspector] public GameObject instanceOfEnemy;
    public Transform[] spawnPoints;
    private Collider[] nearbyColliders;
    private Enemy enemyScript;
    private EnemyController enemyController;
    private FlockAgent flockAgent;

	//TODO: write docs
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ID"></param>
    public void Setup(int ID)
    {
        enemyController = instanceOfEnemy.GetComponent<EnemyController>();
        enemyScript = instanceOfEnemy.GetComponent<Enemy>();
        flockAgent = instanceOfEnemy.GetComponent<FlockAgent>();
        enemyScript.SetIdentifier(ID);
    }

    public int GetID()
    {
        return enemyScript.GetIdentifier();
    }

	/// <summary>
	/// Calculates the spawn point for this enemy as a random offset from the predetermined spawn locaiton
	/// 
	/// For each collider within the radius of this enemy, ensure that the spawn point does not overlap. Generate new spawn offset if it does and try again.
	/// </summary>
    public Transform CalculateSpawnPoint()
    {
		const int Radius = 2;
        bool foundSpawnPoint;
        foundSpawnPoint = false;
        int spawnPointRef = Random.Range(0, spawnPoints.Length - 1);
        Vector3 originalSpawnPoint = spawnPoints[spawnPointRef].position;
        nearbyColliders = Physics.OverlapSphere(spawnPoints[spawnPointRef].position, Radius);
        if(!Physics.CheckSphere(spawnPoints[spawnPointRef].position, Radius))
        {
            foundSpawnPoint = true;
            Vector2 spawnOffset = Random.insideUnitCircle * Radius;
            Vector3 newSpawnpoint = new Vector3(originalSpawnPoint.x + spawnOffset.x, 0, originalSpawnPoint.z + spawnOffset.y);
            spawnPoints[spawnPointRef].position = newSpawnpoint;
        }
      
        while (!foundSpawnPoint)
        {
            Vector2 spawnOffset = Random.insideUnitCircle * Radius;
            Vector3 newSpawnpoint = new Vector3(originalSpawnPoint.x + spawnOffset.x, 0, originalSpawnPoint.z + spawnOffset.y);
            spawnPoints[spawnPointRef].position = newSpawnpoint;
            if (nearbyColliders[0] == null)
            {
                foundSpawnPoint = true;
            }
            foreach (Collider collider in nearbyColliders)
            {
				foundSpawnPoint = true;
                if (collider.bounds.Contains(newSpawnpoint) && collider.tag != "Ground")
                {
					foundSpawnPoint = false;
					break;
				}
            }
        }
        return spawnPoints[spawnPointRef];
    }

    public FlockAgent GetFlockAgent()
    {
        return flockAgent;
    }

	/// <summary>
	/// Stuns this enemy
	/// </summary>
    public void Stun()
    {
        enemyScript.StunCoroutineWrapper();
    }

}
