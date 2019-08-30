using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    public int health;

	private GameObject playerGO;
	private Player player;
	private Transform playerPos;
    [SerializeField] private EnemyController enemyController;
    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
		playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        playerPos = playerGO.transform;
        agentCollider = GetComponent<Collider>();
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
	/// <term>Bullet</term>
	/// <description>
	/// Reduce health 
	/// </description>
	/// </item>
	/// </list>
	/// </summary>
	/// <param name="dmg"></param>
    public void takeDamage(int dmg)
    {
        health = health - dmg;
    }


	public void OnDisable()
	{
		enemyController.enabled = false;
	}

	public void OnEnable()
	{
		enemyController.enabled = true;
	}

    /// <summary>
    /// Moves the agent by calculating a distance uing a vecotr and time
    /// </summary>
    /// <param name="velocity"> the vector along which the agent will move </param>
    public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }
}
