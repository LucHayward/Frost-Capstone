using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FilteredFlockBehaviour
{
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
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            if(Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                numberOfObjectsToAvoid++;            
                avoidanceMove += (agent.transform.position - item.position);       
            }
        }
        avoidanceMove = avoidanceMove.normalized;
        Debug.DrawRay(agent.transform.position, avoidanceMove, Color.red);
        return avoidanceMove;
    }
}
