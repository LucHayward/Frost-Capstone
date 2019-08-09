using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Transform playerPos;
    private Player player;
    public int lvl;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter3D(Collider collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health--;
            if (health < 1)
            {
                print("dead");
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
