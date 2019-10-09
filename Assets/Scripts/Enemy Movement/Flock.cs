using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockBehaviour behaviour;
    [Range(1f, 100f)]
    public float baseSpeedFactor = 1f;
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;
    float squareMaxSpeed;
    float squareNeighbourRadius;
	float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    private float maxSpeed;

    void Start()
    {
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
    }

	/// <summary>
	/// Moves every flock agent by the calculated amount each frame
	/// </summary>
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

				maxSpeed *= baseSpeedFactor;
				squareMaxSpeed = maxSpeed * maxSpeed;
                List<Transform> context = GetNearbyObjects(agent);
                Vector3 move = behaviour.CalculateMove(agent, context, this);
                move *= baseSpeedFactor; 
                if (move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * maxSpeed; /// make it one then times by max speed
                }

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
			if ((c.tag == "Enemy" && c != agent.AgentCollider) || (c.tag == "Obstacle"))
            {
                context.Add(c.transform);
            }
        }
        return context;
	}
}
