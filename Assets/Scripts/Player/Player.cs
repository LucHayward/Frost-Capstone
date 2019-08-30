using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Player : MonoBehaviour
{
    
    public float speed;
    private Vector3 velocity;
    public int health=10;
    public TextMeshProUGUI healthText;
    public bool hasShield=false;
    public int lvl;
    public bool isFast=false;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    // Start is called before the first frame update
    void Start()
    {
        health = 10 + lvl * 2;
    }

    // Update is called once per frame
    void Update()
    {
		if (health <= 0)
		{
			Debug.Log("Player Died");
			OnDeath();
		}
	}

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
