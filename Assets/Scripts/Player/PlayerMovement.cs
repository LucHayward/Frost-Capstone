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

	public Transform cameraTransform;
    private Vector3 moveDirection;

    public float speedMultiplier = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

	[Range (0,1)]
	public float turnSpeed;

	public Vector3 debugVelocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

	/// <summary>
	/// 
	/// </summary>
    void Update()
    {
		
		//Debug.Log("IsGorunded: "+ characterController.isGrounded);
		if (characterController.isGrounded)
        {

            moveDirection = new Vector3();

            Vector3 forward = cameraTransform.forward;
            forward *= Input.GetAxis("Vertical");

            Vector3 right = cameraTransform.right;
            right *= Input.GetAxis("Horizontal");

            moveDirection += forward;
            moveDirection += right;

			if(moveDirection.sqrMagnitude > 1)
			{
				//Debug.Log("Diagonal Movement");
				moveDirection.Normalize();
			}

            moveDirection *= speedMultiplier;




			Vector3 localVelocity = transform.InverseTransformDirection(characterController.velocity);
			//debugVelocity = localVelocity;
			float speed = localVelocity.magnitude;
			animator.SetFloat("velocity", speed);
			animator.SetFloat("velocityX", localVelocity.x);
			animator.SetFloat("velocityZ", localVelocity.z);

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

			if (Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
				animator.SetBool("isJump", true);
			}
			else
			{
				animator.SetBool("isJump", false);
			}
			moveDirection.y = -characterController.stepOffset/Time.deltaTime;
        }
        else
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            forward *= Input.GetAxis("Vertical") * speedMultiplier;

            Vector3 right = transform.TransformDirection(Vector3.right);
            right *= Input.GetAxis("Horizontal") * speedMultiplier;

            moveDirection.x = forward.x + right.x;
            moveDirection.z = forward.z + right.z;
            moveDirection.y -= gravity * Time.deltaTime;

        }


        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }


}
