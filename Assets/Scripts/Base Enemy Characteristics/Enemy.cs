using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    public int health;
    private int identificationNumber;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private FlockAgent flockAgent;

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
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ID"></param>
    public void setIdentifier(int ID)
    {
        identificationNumber = ID;
    }

    public int getIdentifier()
    {
        return identificationNumber;
    }

	/// <summary>
    /// 
    /// </summary>
    /// <param name="dmg"></param>
    public void takeDamage(int dmg)
    {
        health = health - dmg;
    }


	public void OnDisable()
	{
		enemyController.enabled = false;
        flockAgent.enabled = false;
	}

	public void OnEnable()
	{
		enemyController.enabled = true;
        flockAgent.enabled = true;
	}
}
