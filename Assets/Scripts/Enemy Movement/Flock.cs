using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockBehaviour behaviour;
    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;
    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    private float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
		//TODO I think we should up this
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent agent in GameManager.Get().agents)
		{
            if(agent != null)
            {
                string type = agent.getType();
                if (type == "Witch")
                    maxSpeed = 1.5f;
                else if(type == "Vampire")
                    maxSpeed = 1.5f;
                else
                    maxSpeed = 5;
                squareMaxSpeed = maxSpeed * maxSpeed;
                List<Transform> context = GetNearbyObjects(agent);
                Vector3 move = behaviour.CalculateMove(agent, context, this);
                move *= driveFactor; //Is this ever needed?
                if (move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * maxSpeed; /// make it one then times by max speed
                }

				Debug.DrawRay(agent.transform.position, move, Color.magenta);
                agent.Move(move);
            }
		}
    }

    /// <summary>
    /// Populates a list with the transforms of nearby object.
    /// </summary>
    /// <param name="agent"> the agent around which the search will take place</param>
    /// <returns> A list of trasnforms of gameobjects around the agent </returns>
    List<Transform> GetNearbyObjects(FlockAgent agent)
	{
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighbourRadius);
		
        foreach(Collider c in contextColliders)
        {
            if(c != agent.AgentCollider || c.tag == "Obstacle")
            {
                context.Add(c.transform);
            }
        }
        return context;
	}

	private void OnDrawGizmos()
	{
		foreach (FlockAgent agent in GameManager.Get().agents)
		{
			if (agent != null)
			{
				Gizmos.DrawWireSphere(agent.transform.position, neighbourRadius);
				//Gizmos.color = Color.red;
				//Gizmos.DrawWireSphere(agent.transform.position, neighbourRadius*avoidanceRadiusMultiplier);
				//Gizmos.color = Color.white;
			}
		}
	}


}
