using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
	Collider agentCollider;
	public Collider AgentCollider { get { return agentCollider; } }
	public string type;

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
		if (this.enabled)
		{
			float m = velocity.magnitude;
			velocity = Vector3.ProjectOnPlane(velocity, Vector3.up).normalized * m;
			if (velocity != Vector3.zero)
			{
				transform.forward = velocity;
			}

			transform.position += velocity * Time.deltaTime;
		}
	}
}
