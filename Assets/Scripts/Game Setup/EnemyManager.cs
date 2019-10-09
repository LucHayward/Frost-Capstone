using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles the instantiation and setup of the Enemy as well as acting as a container for the various player components
/// </summary>
[Serializable]
public class EnemyManager
{
	[HideInInspector] public GameObject instanceOfEnemy;
	public Transform[] spawnPoints;
	private Collider[] nearbyColliders;
	private Enemy enemyScript;
	private EnemyController enemyController;
	private FlockAgent flockAgent;
    
    private int unlockedAreas = 0;
    private GameManager gameManager;

	/// <summary>
	/// Sets up the enemy components after the Game manager is ready to load them into the scene.
	/// </summary>
	/// <param name="ID">Random identifier</param>
	public void Setup(int ID)
	{
        gameManager = GameManager.Get();
		enemyController = instanceOfEnemy.GetComponent<EnemyController>();
		enemyScript = instanceOfEnemy.GetComponent<Enemy>();
		flockAgent = instanceOfEnemy.GetComponent<FlockAgent>();
		enemyScript.SetIdentifier(ID);
	}
    

 

	/// <summary>
	/// Returns the random id of this enemy
	/// </summary>
	/// <returns></returns>
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
        int spawnPointRef;
        

        //Single original spawn point
        spawnPointRef = 0;
        //All areas unlocked
        if (unlockedAreas == 3)
        {
            spawnPointRef = Random.Range(0, spawnPoints.Length - 1);
        }
        //Castle unlocked
        else if (unlockedAreas == 2)
        {
            spawnPointRef = Random.Range(0, 3);
        }
        //Graveyard unlocked
        else if (unlockedAreas == 1) {
            spawnPointRef = Random.Range(0, 2);          
        }

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

    public void SetUnlocked(int numberOfAreas)
    {
        unlockedAreas = numberOfAreas;
    }

}
