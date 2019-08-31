using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider>();
    }
    /// <summary>
    /// Moves the agent by calculating a distance uing a vecotr and time
    /// </summary>
    /// <param name="velocity"> the vector along which the agent will move </param>
    public void Move(Vector3 velocity)
    {
        if(this.enabled)
        {
            transform.forward = velocity;
            transform.position += velocity * Time.deltaTime;
        }   
    }
}
