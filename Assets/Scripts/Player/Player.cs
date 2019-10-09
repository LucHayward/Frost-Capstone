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
    private int maxHealth = 100;

    public Slider healthSlider;
    public Slider blueSlider;
    public Slider greenSlider;
    public Slider redSlider;

    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
	public int level;
	public TextMeshProUGUI healthText;
    public bool hasShield;
    public bool isFast;
    public bool lowHP = false;

    public AudioSource playerTakeDamageAudio;
    public AudioSource playerDeathAudio;

    //types of frost essence
    public float blueFE = 0;
    public float greenFE = 0;
    public float redFE = 0;

    Unlockable[] doors;

    //Audio
    public AudioSource lowHealth;

    //used for frost essence abilities
    public bool isGreen = false;
    public bool isBlue = false;
    public bool isRed = false;

    private int score;
    private bool isDamaged;
    private bool alive = true;

    [SerializeField] private PlayerMovement playerMovement = null; //Assigned in inspector
	[SerializeField] private PlayerAttack playerAttack = null; //Assigned in inspector

    private bool isPlayer1;

    public void SetPlayerNum(int i)
    {
        isPlayer1 = i == 0;

        damageImage = GameObject.Find("P" + (i + 1) + "_"+"DamageImage").GetComponent<Image>();

        GameObject healthUI = GameObject.Find("P"+(i+1)+"_"+"HealthUI");
        healthSlider = healthUI.GetComponentInChildren<Slider>();

        GameObject blueUI = GameObject.Find("P" + (i + 1) + "_" + "BlueFeUI");
        blueSlider = blueUI.GetComponentInChildren<Slider>();
        blueSlider.value = 0;

        GameObject greenUI = GameObject.Find("P"+(i+1)+"_"+"GreenFeUI");
        greenSlider = greenUI.GetComponentInChildren<Slider>();
        greenSlider.value = 0;

        GameObject redUI = GameObject.Find("P"+(i+1)+"_"+"RedFeUI");
        redSlider = redUI.GetComponentInChildren<Slider>();
        redSlider.value = 0;

        MeleeAttack meleeAttack = GetComponentInChildren<MeleeAttack>();
        meleeAttack.setPlayerNum(i);
    }

    private void Awake()
    {
        currrentHealth = startingHealth;
    }

    private void Start()
    {


        
        doors = FindObjectsOfType<Unlockable>();

        GameObject damageItem = GameObject.Find("Damage");
        damageImage = damageItem.GetComponentInChildren<Image>();
    }


    private void Update()
    {
        if (currrentHealth <= 20)
        {
            LowHealth();
        }
        else
        {
            lowHealth.Pause();
            lowHP = false;
            
        }

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

        if (isBlue)
        {
            if (blueFE > 0)
            {
                playerMovement.SetSpeed(10.0f);
                blueFE -=0.3f;
                blueSlider.value = blueFE;
            }
            else
            {
                playerMovement.SetSpeed(6.0f);
                isBlue = false;
            }
            
        }
        if (isRed)
        {
            if (redFE > 0)
            {
                redFE -= 0.3f;
                redSlider.value = redFE;
                
            }
            else
            {
                isRed = false;
            }

        }

        if (Input.GetButtonDown(isPlayer1 ? "P1_Interact" : "P2_Interact"))
        {
            foreach(Unlockable door in doors)
            {
                if (door.isUnlockable)
                {
                    float dist = Vector3.Distance(door.gameObject.transform.position, transform.position);

                    if (dist <= 10.0f)
                    {
                        door.UnlockDoor();
                    }
                }
                
            }
        }



    }

    /// <summary>
    /// Reduce player health and perform death check
    /// </summary>
    /// <param name="damageAmount"> the amount of health to remove from the player</param>
    public void TakeDamage(int damageAmount)
	{
        isDamaged = true;
        playerTakeDamageAudio.Play();
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

    //For low health Audio
    public void LowHealth()
    {
        if (lowHP == false)
        {
            lowHealth.Play();
            lowHP = true;
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
        playerDeathAudio.Play();
		playerMovement.RandomizeDeathVariant();
		playerMovement.animator.SetTrigger("die");

        //TODO: Disable colliders and movement scripts etc
        GameManager.Get().HandlePlayerDeath(isPlayer1 ? 0 : 1);
        playerAttack.enabled = false;
        playerMovement.enabled = false;

        alive = false;
    }

    public void Respawn()
    {
        alive = true;
        playerAttack.enabled = true;
        playerMovement.enabled = true;

        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.transform.position = Vector3.zero;
        gameObject.GetComponent<CharacterController>().enabled = true;

        playerMovement.animator.SetTrigger("respawn");
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
        alive = true;
    }

    private void CapFrostEssence(string eType)
    {
        if(eType == "Blue")
        {
            if (blueFE < 0)
            {
                blueFE = 0;
            }

            else if (blueFE > 100)
            {
                blueFE = 100.0f;
            }
        }

        if(eType == "Green")
        {
            if (greenFE < 0)
            {
                greenFE = 0;
            }

            else if (greenFE > 100)
            {
                greenFE = 100.0f;
            }
        }

        if(eType == "Red")
        {
            if (redFE < 0)
            {
                redFE = 0;
            }

            else if (redFE > 100)
            {
                redFE = 100.0f;
            }
        }
        
       
    }
    public void UseFrostEssence(string type)
    {
        if(type == "Blue")
        {
            isBlue = true;
        }

        if(type == "Red")
        {
            isRed = true;
        }

        if(type == "Green")
        {
            int heal = (int) greenFE;
            GainHealth(heal);
            greenFE = 0;
            greenSlider.value = greenFE;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Essence"))
        {
            GainFrostEssence(other.gameObject.GetComponent<FrostEssence>().GetEssenceType(), other.gameObject.GetComponent<FrostEssence>().GetEssenceAmount());
        }
    }

    public void GainFrostEssence(string essenceType, float amount)
    {
        if(essenceType == "Blue")
        {
            blueFE += amount;
            CapFrostEssence(essenceType);
            blueSlider.value = blueFE;
        }
        else if(essenceType == "Green")
        {
            greenFE += amount;
            CapFrostEssence(essenceType);
            greenSlider.value = greenFE;
        }
        else if(essenceType == "Red")
        {
            redFE += amount;
            CapFrostEssence(essenceType);
            redSlider.value = redFE;
        }
    }

    public void GainHealth(int amount)
    {
        currrentHealth += amount;
        if(currrentHealth > maxHealth)
        {
            currrentHealth = maxHealth;
        }
        healthSlider.value = currrentHealth;
    }
    public bool IsAlive()
    {
        return alive;
    }


    

}
