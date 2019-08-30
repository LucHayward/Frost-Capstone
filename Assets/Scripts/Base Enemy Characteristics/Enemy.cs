using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public int health;

	private GameObject playerGO;
	private Player player;
	private Transform playerPos;

    Vector3 velocity;
    private Transform prevTransform;
	// Start is called before the first frame update
	void Start()
    {
        prevTransform = transform;
		playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        playerPos = playerGO.transform;
    }

    // Update is called once per frame
    void Update()
    {

        

        if (health < 1)
        {
            print("dead");
            Destroy(gameObject);
        }
    }
	
	/// <summary>
	/// Handle interaction with triggers.
	/// <list type="Bullet">
	/// <item>
	///		<term>Bullet</term>
	///		<description>
	///		Reduce health and initiate "death" if health is less than 1
	///		</description>
	/// </item>
	/// </list>
	/// </summary>
	/// <param name="collision"></param>
    public void takeDamage(int dmg)
    {
        health = health - dmg;
    }





}
