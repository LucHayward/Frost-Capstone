using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Player : MonoBehaviour
{
    public int health;
	public int lvl;

	public TextMeshProUGUI healthText;
    public bool hasShield=false;
    public bool isFast=false;

    [SerializeField] private PlayerMovement playerMovement = null; //Assigned in inspector
	[SerializeField] private PlayerAttack playerAttack = null; //Assigned in inspector

	// Start is called before the first frame update
	void Start()
    {
        health += lvl * 2;
    }

	/// <summary>
	/// Reduce player health and perform death check
	/// </summary>
	/// <param name="dmg"> the amount of health to remove from the player</param>
	public void TakeDamage(int dmg)
	{
        Debug.Log("Took " + dmg + " damage");
		health -= dmg;
		if (health <= 0)
		{
			Debug.Log("Player Died");
			OnDeath();
		}
	}

	/// <summary>
	/// Monitor player health
	/// </summary>
    void Update()
    {
		// TODO: remove this if still redundant in TakeDamage()
		if (health <= 0)
		{
			Debug.Log("Player Died");
			OnDeath();
		}
	}

	/// <summary>
	/// Handle player death sequence
	/// <list type="bullet">
	/// <item>
	///		<term>Animation</term>
	///		<description>
	///		Play death animation
	///		</description>
	/// </item>
	/// <item>
	///		<term>Determine Game Over</term>
	///		<description>
	///		Single Player: Initiate game over sequence
	///		Co-op: Place player in limbo and shift to single player view.
	///		</description>
	/// </item>
	/// </list>
	/// </summary>
	private void OnDeath()
	{

		Instantiate(gameObject);
		Destroy(gameObject);
	}

    public void OnDisable()
    {
        playerMovement.enabled = false;
        playerAttack.enabled = false;
    }

    public void OnEnable()
    {
        playerMovement.enabled = true;
        playerAttack.enabled = true;
    }
}
