using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Player : MonoBehaviour
{
    
    public float speed;
    public int health=10;
	public int lvl;

	public TextMeshProUGUI healthText;
    public bool hasShield=false;
    public bool isFast=false;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    // Start is called before the first frame update
    void Start()
    {
        health = 10 + lvl * 2;
    }

 
	/// <summary>
	/// Monitor player health
	/// </summary>
    void Update()
    {
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
