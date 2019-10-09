using UnityEngine;

/// <summary>
/// Scripting for FrostEssence pickups
/// </summary>
public class FrostEssence : MonoBehaviour
{
	// type of drop (colour)
	public string type;

	private float amount;

	private Player[] players;

	void Start()
	{
		//TODO: particle effects
		players = FindObjectsOfType<Player>();
		Destroy(gameObject, 15);
	}

	private void Update()
	{
		for (int i = 0; i < players.Length; i++)
		{
			float dist = Vector3.Distance(players[i].transform.position, gameObject.transform.position);
			if (dist <= 1.5f)
			{
				TransferEssence(players[i].gameObject, amount);
			}
		}
	}

	public void TransferEssence(GameObject player, float amount)
	{
		if (type == "Blue")
		{
			player.GetComponent<Player>().GainFrostEssence(type, amount);
			Destroy(gameObject);
		}
		else if (type == "Green")
		{
			player.GetComponent<Player>().GainFrostEssence(type, amount);
			Destroy(gameObject);
		}
		else if (type == "Red")
		{
			player.GetComponent<Player>().GainFrostEssence(type, amount);
			Destroy(gameObject);
		}
	}

	public void SetAmount(float value)
	{
		amount = value;
	}

	public string GetEssenceType()
	{
		return type;
	}

	public float GetEssenceAmount()
	{
		return amount;
	}
}
