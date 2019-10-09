using UnityEngine;

/// <summary>
/// Control script for the health drop items
/// </summary>
public class HealthDrop : MonoBehaviour
{
	private int amount = 20;

	Player[] players;

	void Start()
	{
		players = FindObjectsOfType<Player>();
	}

	private void Update()
	{
		for (int i = 0; i < players.Length; i++)
		{
			float dist = Vector3.Distance(players[i].transform.position, gameObject.transform.position);
			if (dist <= 1.5f)
			{
				TransferHealth(players[i].gameObject, amount);
			}
		}
	}


	public void TransferHealth(GameObject player, int amount)
	{
		player.GetComponent<Player>().GainHealth(amount);
		HealthSpawn[] spawn = FindObjectsOfType<HealthSpawn>();
		for (int i = 0; i < 4; i++)
		{
			float dist = Vector3.Distance(gameObject.transform.position, spawn[i].gameObject.transform.position);
			if (dist <= 2.0f)
			{
				spawn[i].HealthUsed();
			}
		}
		Destroy(gameObject);
	}
}
