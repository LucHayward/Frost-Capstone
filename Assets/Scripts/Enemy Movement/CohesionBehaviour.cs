﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohesionBehaviour : FlockBehaviour
{
	public float agentSmoothFactor;

	/// <summary>
	/// Finds middle point between all neighbours and tries to move there (smoothed version).
	/// </summary>
	/// <param name="agent"> the current agent </param>
	/// <param name="context"> the neighbouring objects </param>
	/// <param name="flock"> the flock the agent is in </param>
	/// <returns> the average position </returns>
	public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
	{
		///if there are no neighbours return no adjustment
		if (context.Count == 0)
			return Vector3.zero;

		///add all points together and average
		Vector3 cohesiveMove = Vector3.zero;
		int usedTransformCnt = 0;
		foreach (Transform item in context)
		{
			if (item.CompareTag("Enemy"))
			{
				cohesiveMove += item.position;
				usedTransformCnt++;
			}
		}
		cohesiveMove /= usedTransformCnt;

		///create offset from agent position
		cohesiveMove -= agent.transform.position;
		cohesiveMove = cohesiveMove.normalized;

		cohesiveMove = Vector3.Lerp(agent.transform.forward, cohesiveMove, agentSmoothFactor);

		return cohesiveMove;
	}
}
