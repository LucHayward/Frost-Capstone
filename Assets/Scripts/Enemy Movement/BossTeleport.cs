using UnityEngine;

public class BossTeleport : MonoBehaviour
{
	private GameObject playerGameObject;
	private Transform playerTrasnform;
	private float currentTime;
	private float lastTeleportTime;
	public float teleportDelay;

	void Start()
	{
		playerGameObject = GameObject.FindGameObjectWithTag("Player");
		playerTrasnform = playerGameObject.transform;
	}

	void Update()
	{
		currentTime = Time.time;

		// Check if basic attack is on cooldown (attack speed)
		if (currentTime - lastTeleportTime > teleportDelay)
		{
			lastTeleportTime = currentTime;
			Teleport();
		}

	}

	/// <summary>
	/// Teleports the boss npc to a random point on a circumfernece around the player. This will be triggered when the boss is taking damage too quickly
	/// </summary>
	public void Teleport()
	{
		float radius = 5f;
		Vector3 originPoint = playerTrasnform.position;
		float newBossPositionX = originPoint.x + Random.Range(-radius, radius);
		float newBossPositionZ = originPoint.z + Random.Range(-radius, radius);
		gameObject.transform.position = new Vector3(newBossPositionX, playerTrasnform.position.y, newBossPositionZ);
	}
}
