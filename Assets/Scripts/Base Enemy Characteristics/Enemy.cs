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
    public Animator animator;
    Vector3 velocity;
    
    private Vector3 prevTransform;
    float v;
	// Start is called before the first frame update
	void Start()
    {
        prevTransform = transform.position;
		playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        playerPos = playerGO.transform;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = ((transform.position - prevTransform) / Time.deltaTime);
        v = velocity.magnitude;
        animator.SetFloat("Velocity", v);

        prevTransform = transform.position;
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
	/// <term>Bullet</term>
	/// <description>
	/// Reduce health 
	/// </description>
	/// </item>
	/// </list>
	/// </summary>
	/// <param name="collision"></param>
    public void takeDamage(int dmg)
    {
        health = health - dmg;
    }





}
