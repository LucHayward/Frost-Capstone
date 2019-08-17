using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyJ : MonoBehaviour
{
    public float speed;
    private Transform playerPos;
    private PlayerJ player;
    public int lvl;
    public int health;
    
    // Start is called before the first frame update
    void Start()
    {
        speed = lvl * 2 + 12;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerJ>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direct = transform.position - playerPos.position;

        transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed*Time.deltaTime);

        float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player.hasShield == false)
            {
                player.health--;
            }
            
            
        }

        if(collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
