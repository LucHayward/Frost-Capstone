﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    void Start()
    {
        characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

    void Update()
    {
		//TODO; implement jump?
		//animator.SetBool("IsJump", false);
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
				Debug.Log("Diagonal Movement");
				moveDirection.Normalize();
			}

            moveDirection *= speedMultiplier;




			Vector3 velocity = characterController.velocity;
			float speed = velocity.magnitude;
			animator.SetFloat("velocity", speed);
			animator.SetFloat("velocityX", characterController.velocity.x);
			animator.SetFloat("velocityZ", characterController.velocity.z);

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
			//if (Input.GetButton("Jump"))
			//{
			//moveDirection.y = jumpSpeed;
			//animator.SetBool("IsJump", true);
			//}
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
        //animator.SetFloat("Speed", moveDirection.sqrMagnitude);
		// TODO: Change the animation controller to work using speed for run/idle transitions
    }


}
