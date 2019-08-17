using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Boss : MonoBehaviour
{
    public float speed;
    private Transform playerPos;
    private PlayerJ player;
    public int lvl;
    public int health;
    private Transform bossPos;
    public float startTime;
    public float endTime;
    public bool isDead = false;
    public TextMeshProUGUI bossHP;
    private EnemyJ enemy;
    private int direction = 1; //int direction where 0 is stay, 1 up, -1 down
    
    
    // Start is called before the first frame update
    void Start()
    {
        bossHP = GameObject.FindGameObjectWithTag("BossHP").GetComponent<TextMeshProUGUI>();

        isDead = false;
        health = lvl * 10 + 20;
        speed = lvl * 5 + 10;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerJ>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyJ>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        bossPos = GameObject.FindGameObjectWithTag("Boss").transform;
    }

    // Update is called once per frame
    void Update()
    {
        bossHP.text = "BossHP: " + health.ToString();
        
        if (health <= 0)
        {
            isDead = true;
            Destroy(gameObject);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            health--;
        }
    }

    
}
