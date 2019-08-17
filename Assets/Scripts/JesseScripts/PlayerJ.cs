using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class PlayerJ : MonoBehaviour
{
    
    public float speed;
    private Vector2 velocity;
    public int health=10;
    private Rigidbody2D rb;
    public TextMeshProUGUI healthText;
    public bool hasShield=false;
    public int lvl;
    public bool isFast=false;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            lvl = 1;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            lvl = 2;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            lvl = 3;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            lvl = 4;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            lvl = 5;
        }

        health = 10 + lvl * 2;
        rb = GetComponent<Rigidbody2D>();

        healthText = GameObject.FindGameObjectWithTag("HealthTxt").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene(7);
        }
        Vector2 movementInp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        velocity = movementInp.normalized * speed;
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BossBullet"))
        {
            if (hasShield == false)
            {
                health--;
                Destroy(collision.gameObject);
            }


        }
        
    }

  



}
