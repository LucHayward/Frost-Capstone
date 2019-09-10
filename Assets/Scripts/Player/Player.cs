using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Player : MonoBehaviour
{
    public int startingHealth = 100;
    public int currrentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
	public int level;
	public TextMeshProUGUI healthText;
    public bool hasShield;
    public bool isFast;

    private bool isDamaged;

    [SerializeField] private PlayerMovement playerMovement = null; //Assigned in inspector
	[SerializeField] private PlayerAttack playerAttack = null; //Assigned in inspector

    private void Awake()
    {
        currrentHealth = startingHealth;
    }

    private void Start()
    {
        GameObject healthUI = GameObject.Find("HealthUI");
        healthSlider = healthUI.GetComponentInChildren<Slider>();
        GameObject damageItem = GameObject.Find("Damage");
        damageImage = damageItem.GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (isDamaged)
        {
            damageImage.color = flashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        isDamaged = false;

        // TODO: remove this if still redundant in TakeDamage()
        if (currrentHealth <= 0)
        {
            Debug.Log("Player Died");
            OnDeath();
        }
    }

    /// <summary>
    /// Reduce player health and perform death check
    /// </summary>
    /// <param name="damageAmount"> the amount of health to remove from the player</param>
    public void TakeDamage(int damageAmount)
	{
        isDamaged = true;

		// If the player is currently in it's hit animation don't set trigger again.
		if (playerMovement.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit Reaction Blend Tree"))
		{
			
			playerMovement.RandomizeReactHitVariant();
			playerMovement.animator.SetTrigger("takeDamage");
		}

		Debug.Log("Took " + damageAmount + " damage");
		currrentHealth -= damageAmount;
        healthSlider.value = currrentHealth;
		if (currrentHealth <= 0)
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
		playerMovement.RandomizeDeathVariant();
		playerMovement.animator.SetTrigger("die");

		//TODO: Disable colliders and movement scripts etc

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
