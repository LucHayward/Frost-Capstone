using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyManager
{
    [HideInInspector] public GameObject instanceOfEnemy;
    public Transform spawnPoint;
    private Collider[] nearbyColliders;
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
        flockAgent.enabled = true;
        enemyController.enabled = true;
    }

    public void DisableMovement()
    {
        flockAgent.enabled = false;
        enemyController.enabled = false;
    }


	/// <summary>
	/// Calculates the spawn point for this enemy as a random offset from the predetermined spawn locaiton
	/// 
	/// For each collider within the radius of this enemy, ensure that the spawn point does not overlap. Generate new spawn offset if it does and try again.
	/// </summary>
    public void CalculateSpawnPoint()
    {
		const int Radius = 10;
		nearbyColliders = Physics.OverlapSphere(spawnPoint.position, Radius);
        bool foundSpawnPoint = false;
        //Vector3 offsetSpawnPoint = Vector3.zero;
        while (!foundSpawnPoint)
        {
            Vector2 spawnOffset = Random.insideUnitCircle * Radius;
            spawnPoint.position = new Vector3(spawnPoint.position.x + spawnOffset.x, 0, spawnPoint.position.y + spawnOffset.y);
			

            foreach (Collider collider in nearbyColliders)
            {
				foundSpawnPoint = true;
                if (collider.bounds.Contains(spawnPoint.position))
                {
					foundSpawnPoint = false;
					break;
				}
            }
        }
    }

    public FlockAgent GetFlockAgent()
    {
        return flockAgent;
    }
}
