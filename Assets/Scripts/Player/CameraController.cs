using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform orbitTarget;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;

	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	[Range(0, 1)]
	public float cameraSmoothingFactor;
	public Transform bufferPos;


	public float distanceMin = .5f;
	public float distanceMax = 15f;

	private Rigidbody rb;
	private float desiredDistance; // Allows for return after pulling camera in front of geometry
	private bool canResetDistance = true;

	float x = 0.0f;
	float y = 0.0f;

	// Use this for initialization
	void Start()
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

		rb = GetComponent<Rigidbody>();

		// Make the rigid body not change rotation
		if (rb != null)
		{
			rb.freezeRotation = true;
		}

		desiredDistance = distance;
	}

	void LateUpdate()
	{
		canResetDistance = true;
		if (orbitTarget)
		{
			x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			Quaternion rotation = Quaternion.Euler(y, x, 0);

			// TODO Do we want to be able to zoom in/out with the camera
			float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
			if (scrollAmount != 0)
			{
				distance = Mathf.Clamp(distance - scrollAmount*5, distanceMin, distanceMax);
				desiredDistance = Mathf.Clamp(desiredDistance - scrollAmount*5, distanceMin, distanceMax);
			}


			// Handle preventing the camera being BEHIND an object
			RaycastHit hit;
			if (distance>desiredDistance)
			{
				distance = desiredDistance;
			} else if (Physics.Linecast(orbitTarget.position, transform.position, out hit))
			{
				// Allow enemy and player to pass in front of camera target
				if (!(hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy")))
				{
					distance =  hit.distance;
				}
			} else if (!Mathf.Approximately(distance, desiredDistance)) // If the camera is not at the desired distance, perform a raycast backwards to check for intersections.
			{
				//RaycastHit backhit;
				if (Physics.Linecast(transform.position, bufferPos.position, out hit))
				{
					distance += hit.distance;
				}
				else
				{
					distance = Mathf.SmoothStep(distance, desiredDistance, cameraSmoothingFactor);
				}
			}

			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + orbitTarget.position;

			transform.rotation = rotation;
			transform.position = position;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}



