using System;
using UnityEngine;

/// <summary>
/// Controls the camera
/// Enables the camera to orbit the player
/// Dynamically adjusts the camera distance to ensure the camera neither enters the environment
/// nor is the player occluded by any obstacles
/// </summary>
public class CameraController : MonoBehaviour
{
	public Transform orbitTarget;
	public Transform horizontalTransform;
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
	private bool isPlayer1;

	float x = 0.0f;
	float y = 0.0f;

	void Start()
	{
		rb = GetComponent<Rigidbody>();

		// Make the rigid body not change rotation
		if (rb != null)
		{
			rb.freezeRotation = true;
		}

		desiredDistance = distance;
	}

	public void setPlayerNum(int i)
	{
		isPlayer1 = i == 0;
	}

	/// <summary>
	/// Translates mouse/joystick movement into camera orbit around a fixed point.
	/// Prevents environment from passsing in front of camera and player by zooming camera in closer and returning when there is no longer an obstacle
	/// </summary>
	void LateUpdate()
	{
		if (orbitTarget)
		{
			x += Input.GetAxis(isPlayer1 ? "P1_MouseX" : "P2_MouseX") * xSpeed * distance * 0.02f;
			y -= Input.GetAxis(isPlayer1 ? "P1_MouseY" : "P2_MouseY") * ySpeed * 0.02f;

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			Quaternion rotation = Quaternion.Euler(y, x, 0);

			float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
			if (scrollAmount != 0)
			{
				distance = Mathf.Clamp(distance - scrollAmount*5, distanceMin, distanceMax);
				desiredDistance = Mathf.Clamp(desiredDistance - scrollAmount*5, distanceMin, distanceMax);
			}

			// Handles preventing the camera being BEHIND an object
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

			horizontalTransform.rotation = rotation;
			horizontalTransform.position = position;
			Vector3 rot = horizontalTransform.eulerAngles;
			rot.x = 0;
			horizontalTransform.eulerAngles = rot;
		}
	}

	/// <summary>
	/// Returns an angle clamped within the given bounds specified in degrees
	/// </summary>
	/// <param name="angle"> to be clamped </param>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <returns> clamped angle</returns>
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}



