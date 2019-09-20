using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }
    public string type;
    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider>();
    }

    public string getType()
    {
        return type;
    }

    /// <summary>
    /// Moves the agent by calculating a distance using a vector and time
    /// </summary>
    /// <param name="velocity"> the vector along which the agent will move </param>
    public void Move(Vector3 velocity)
    {
        if(this.enabled)
        {
			float m = velocity.magnitude;
			velocity = Vector3.ProjectOnPlane(velocity, Vector3.up).normalized * m;
            transform.forward = velocity;
            transform.position += velocity * Time.deltaTime;
        }   
    }
}
