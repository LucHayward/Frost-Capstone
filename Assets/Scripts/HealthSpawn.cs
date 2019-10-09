using UnityEngine;

/// <summary>
/// Controls the scripting of the health pickup respawns
/// </summary>
public class HealthSpawn : MonoBehaviour
{
	public float healthDropCD = 0.0f;
	public Transform spawnPoint;

	public GameObject healthDropGO;

	private float currentTime = 0.0f;
	private float timeUsed = 0.0f;
	private bool isHealth = false;

	void Start()
	{
		SpawnHealth();
	}

	void Update()
	{
		currentTime = Time.time;

		if (currentTime - timeUsed > healthDropCD)
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
