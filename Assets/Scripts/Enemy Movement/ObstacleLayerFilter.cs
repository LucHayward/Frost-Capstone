using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Obstacle Layer")]
public class ObstacleLayerFilter : ContextFilter
{
    public LayerMask layerMaskmask;

    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original)
        {
            if(layerMaskmask == (layerMaskmask |(1 << item.gameObject.layer))) // checks to see if they are on the same layer
            {
                filtered.Add(item);
            }
        }
        return filtered;
    }
}
