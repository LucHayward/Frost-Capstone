using UnityEngine;

/// <summary>
/// Handles the unlocking of doors
/// </summary>
public class Unlockable : MonoBehaviour
{
	public bool isUnlockable;
	public float scoreNeeded;

	private Vector2 scoreVector;
	private float currentScore;
	private GameManager gameManager;
	

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();

	}

	public void UnlockDoor()
	{
		if (isUnlockable)
		{
			scoreVector = gameManager.GetScore();
			currentScore = scoreVector[0] + scoreVector[1];

			if (currentScore >= scoreNeeded)
			{
				Destroy(gameObject);
			}
		}

	}
}
