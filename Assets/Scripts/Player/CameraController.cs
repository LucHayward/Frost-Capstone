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

	public float distanceMin = .5f;
	public float distanceMax = 15f;

	private Rigidbody rb;

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
	}
	// .205 1.528 -1.58
	void LateUpdate()
	{
		if (orbitTarget)
		{
			x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			Quaternion rotation = Quaternion.Euler(y, x, 0);

			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);

			// Handle preventing the camera being BEHIND an object
			RaycastHit hit;
			if (Physics.Linecast(orbitTarget.position, transform.position, out hit))
			{
				// Allow enemy and player to pass in front of camera target
				if (!(hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy")))
				{
					distance -=  hit.distance;
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



