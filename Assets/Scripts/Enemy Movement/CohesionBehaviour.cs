using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohesionBehaviour : FilteredFlockBehaviour
{
    Vector3 currentVelocity;
    public float agentSmoothTime = 1.5f;

    /// <summary>
    /// Finds middle point between all neighbours and tries to move there (smoothed version).
    /// </summary>
    /// <param name="agent"> the current agent </param>
    /// <param name="context"> the neighbouring objects </param>
    /// <param name="flock"> the flock the agent is in </param>
    /// <returns> the average position </returns>
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
		List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

		///if there are no neighbours return no adjustment
		if (filteredContext.Count == 0)
			return Vector3.zero;

		///add all points together and average
		Vector3 cohesiveMove = Vector3.zero;
        foreach (Transform item in filteredContext)
        {
            cohesiveMove += item.position;
        }
		cohesiveMove /= filteredContext.Count;

        ///create offset from agent position
        cohesiveMove -= agent.transform.position;
        cohesiveMove = cohesiveMove.normalized;
        cohesiveMove = Vector3.SmoothDamp(agent.transform.forward, cohesiveMove, ref currentVelocity, agentSmoothTime);
        Debug.DrawRay(agent.transform.position, cohesiveMove, Color.blue);
        return cohesiveMove;
    }
}
