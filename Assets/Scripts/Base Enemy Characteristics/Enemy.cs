using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int lvl;
    public int health;

	private GameObject playerGO;
	private Player player;
	private Transform playerPos;

	// Start is called before the first frame update
	void Start()
    {
		playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        playerPos = playerGO.transform;
    }

    // Update is called once per frame
    void Update()
    {
		
    }
	
	/// <summary>
	/// Handle interaction with triggers.
	/// <list type="bullet">
	/// <item>
	/// <term>Bullet</term>
	/// <description>
	/// Reduce health and initiate "death" if health is less than 1
	/// </description>
	/// </item>
	/// </list>
	/// </summary>
	/// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
		Debug.Log("TriggerEntered", collision);
        if (collision.CompareTag("Bullet"))
        {
            // if collider is a bullet deal damage
            health--;
            if (health < 1)
            {
                print("dead");
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }



	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("ColliderEnter", collision.collider);

	}


}
