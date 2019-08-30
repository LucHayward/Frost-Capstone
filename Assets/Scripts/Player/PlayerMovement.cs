using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the movement calculations as well as communication with the animation controller for the player
/// </summary>
public class PlayerMovement : MonoBehaviour
{
	CharacterController characterController;
	public Animator animator;
	[SerializeField] private AnimationClip jumpAnim;

	public Transform cameraTransform;

	public float speedMultiplier = 6.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;

	[Range(0, 1)]
	public float turnSpeed;

	public Vector3 debugVelocity;

	private Vector3 moveDirection;
	private float jumpAnimTime;

	void Start()
	{
		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		jumpAnimTime = jumpAnim.length;
	}

	/// <summary>
	/// 
	/// </summary>
	void Update()
	{
		moveDirection = new Vector3();

		Vector3 forward = cameraTransform.forward;
		forward *= Input.GetAxis("Vertical");

		Vector3 right = cameraTransform.right;
		right *= Input.GetAxis("Horizontal");

		moveDirection += forward;
		moveDirection += right;

		if (moveDirection.sqrMagnitude > 1)
		{
			//Debug.Log("Diagonal Movement");
			moveDirection.Normalize();
		}

		moveDirection *= speedMultiplier;

		Vector3 localVelocity = cameraTransform.InverseTransformDirection(characterController.velocity);
		debugVelocity = characterController.velocity;
		float speed = localVelocity.magnitude;
		animator.SetFloat("velocity", speed);

		// Prevents the character from "moving" when against a wall
		if (speed > 0)
		{
			//Turn character
			Quaternion newLookRotation = Quaternion.LookRotation(cameraTransform.forward, Vector3.up);
			newLookRotation.x = 0;
			newLookRotation.z = 0;
			//transform.rotation = Quaternion.Slerp(transform.rotation, newLookRotation, Time.deltaTime * turnSpeed);
			//transform.rotation = Quaternion.Slerp(transform.rotation, newLookRotation, turnSpeed);
			//transform.rotation = Quaternion.Slerp(transform.rotation, newLookRotation, Time.deltaTime * turnSpeed);
			transform.rotation = Quaternion.Lerp(transform.rotation, newLookRotation, turnSpeed);
		}

		//Debug.Log("IsGorunded: "+ characterController.isGrounded);
		if (characterController.isGrounded)
		{
			animator.SetFloat("velocityX", localVelocity.x);
			animator.SetFloat("velocityZ", localVelocity.z);
			
			if (Input.GetButtonDown("Jump"))
			{
				//moveDirection.y = jumpSpeed;
				animator.SetTrigger("jump");
				StartCoroutine(GravityPauseForJump());
			}
			moveDirection.y = -characterController.stepOffset/Time.deltaTime;
		}
		else
		{
			moveDirection.y -= gravity * Time.deltaTime;
		}


		// Move the controller
		characterController.Move(moveDirection * Time.deltaTime);
	}

	IEnumerator GravityPauseForJump()
	{
		float tGrav = gravity;
		gravity = 0;
		yield return new WaitForSeconds(3);
		gravity = tGrav;
	}


}
