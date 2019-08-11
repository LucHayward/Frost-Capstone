using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyAttack : MonoBehaviour
{
	public Transform enemyPos;
	public Transform playerPos;

	private Enemy enemyScript;
	private GameObject enemy;
	private GameObject player;
    private Player playerScript;

    public float coolDown;
    public int abLevel;
    public float shootTime = 0;
    public float waitTime;
    public float abStart;
    public float currentTime;

    // Start is called before the first frame update
    void Start()
    {
		// TODO: Store a reference to the game object as well rather than just the components.
		// Use that to reference anyhting else (See Enemy.cs)
		player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<Player>();

		enemy = GameObject.FindGameObjectWithTag("Enemy");
		enemyScript = enemy.GetComponent<Enemy>();

		enemyPos = enemy.transform;
		playerPos = player.transform;
	}

	// Update is called once per frame
	void Update()
    {
		// Please consider a better way of doing this that isn't going to break if someone leaves the game running forever
		// Current time is unused, 
		// if(currentTime - lastShotTime > shot delay)  do shoot and update shotime or something. 
        currentTime = Time.time;
        

        if (Time.time > shootTime)
        {
            shootTime = Time.time + coolDown;
            attack();
        }
        
    }

	// TODO: Make doc comment here, Fix naming (Methods are Capitals in C#)
	// TODO: Do some testing here please Jesse, you want the distance from the player in move than just the X axis. 
	//		 Probably want to look up Distance() theres a method for it.
	void attack()
    {
        if ((playerPos.position.x - enemyPos.position.x) < 10)
        {
			Debug.Log(playerPos.position.x - enemyPos.position.x);
            print("Attack");
            playerScript.health--;
        }
    }
}
