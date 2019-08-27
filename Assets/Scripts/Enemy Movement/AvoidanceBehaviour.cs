using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FlockBehaviour
{
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
            if(Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
               
                numberOfObjectsToAvoid++;
                avoidanceMove += (agent.transform.position - item.position);
            }
        }
        if(numberOfObjectsToAvoid>0)
        {
            avoidanceMove /= numberOfObjectsToAvoid;
        }
        return avoidanceMove;
    }
}
