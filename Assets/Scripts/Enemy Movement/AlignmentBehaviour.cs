using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class AlignmentBehaviour : FlockBehaviour
{
	public float agentSmoothFactor;


	/// <summary>
	/// Finds the alignment that each agent should have in the flock
	/// </summary>
	/// <param name="agent"> the current agent </param>
	/// <param name="context"> the list of neighbouring objects </param>
	/// <param name="flock"> the current flock </param>
	/// <returns> the ideal alignment </returns>
	public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
		///if there are no neighbours maintain parent alignment
		if (context.Count == 0)
            return agent.transform.forward;


		///add all points tgether and average
		Vector3 alignmentMove = Vector3.zero;
        foreach (Transform item in context)
        {
            if(item.CompareTag("Enemy"))
            {
                alignmentMove += item.transform.forward;
            }
			//Debug.Log(item.transform.forward);
		}
		
        
        alignmentMove = alignmentMove.normalized;
		alignmentMove = Vector3.Lerp(agent.transform.forward, alignmentMove, agentSmoothFactor);

		Debug.DrawRay(agent.transform.position, alignmentMove, Color.green);
        return alignmentMove;
    }
}
