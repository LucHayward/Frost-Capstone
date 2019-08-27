﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    public FlockBehaviour[] behaviours;
    public float[] weights;
    /// <summary>
    /// Combines all behaviour vectors into one to calculate the vector along which
    /// the agent should move
    /// </summary>
    /// <param name="agent"> the current agent </param>
    /// <param name="context"> a list of transforms of the gameobjects surrounding the agent </param>
    /// <param name="flock"> the flock the agent belongs to </param>
    /// <returns> the final vecctor along which the agent should move </returns>
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if(weights.Length!=behaviours.Length)
        {
            Debug.LogError("Data is mismatched " + name, this);
            return Vector3.zero;
        }

        Vector3 move = Vector3.zero;

        for(int i = 0; i < behaviours.Length; i++)
        {
            Vector3 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i];

            if(partialMove != Vector3.zero)
            {
                if(partialMove.sqrMagnitude>weights[i]*weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }
                move += partialMove;
            }
        }
        return move;
    }
}
