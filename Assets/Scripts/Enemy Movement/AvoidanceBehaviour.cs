using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FlockBehaviour
{
	public float agentSmoothFactor;

	/// <summary>
	/// Calcuates the vector that the agent should move along in order to avoid nearby objects
	/// </summary>
	/// <param name="agent"> the current agent </param>
	/// <param name="context"> a list of transforms of gameobjects surrounding the current agent</param>
	/// <param name="flock"> the flock the agent is in</param>
	/// <returns> the vector that the agent should move along </returns>
	public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
	{

		///if there are no neighbours return no adjustment
		if (context.Count == 0)
			return Vector3.zero;

		///add all points together and average
		Vector3 avoidanceMove = Vector3.zero;
		int numberOfObjectsToAvoid = 0;
		foreach (Transform item in context)
		{
			if (Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
			{
				numberOfObjectsToAvoid++;
				if (item.CompareTag("Enemy"))
				{
					avoidanceMove += (agent.transform.position - item.position);
				}
				else if (item.CompareTag("Obstacle"))
				{
					avoidanceMove += 3 * (agent.transform.position - item.position);
				}

			}
		}
		avoidanceMove = avoidanceMove.normalized;
		avoidanceMove = Vector3.Lerp(agent.transform.forward, avoidanceMove, agentSmoothFactor);

		return avoidanceMove;
	}
}
