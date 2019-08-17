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
    private Player player;
    public int lvl;
    public int health;
    private Transform bossPos;
    public float startTime;
    public float endTime;
    public bool isDead = false;
    public TextMeshProUGUI bossHP;
    private Enemy enemy;
    private int direction = 1; //int direction where 0 is stay, 1 up, -1 down
    private int top = 8;
    private int bottom = -8;
    
    // Start is called before the first frame update
    void Start()
    {
        bossHP = GameObject.FindGameObjectWithTag("BossHP").GetComponent<TextMeshProUGUI>();

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
        isDead = false;
        health = lvl * 10 + 20;
        speed = lvl * 5 + 10;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        bossPos = GameObject.FindGameObjectWithTag("Boss").transform;
    }

    // Update is called once per frame
    void Update()
    {
        bossHP.text = "BossHP: " + health.ToString();
        Vector2 direct = transform.position - playerPos.position;

        if (direct.magnitude > 25)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
            
            
        }
        
        else
        {
            if (transform.position.y >= top)
            {
                direction = -1;
            }


            if (transform.position.y <= bottom)
            {
                direction = 1;
            }
            transform.Translate(0, speed * direction * Time.deltaTime, 0);
        }
        
            

        
        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        if (health <= 0)
        {
            isDead = true;
            Destroy(gameObject);
            EnemySpawn.isBoss = false;
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
