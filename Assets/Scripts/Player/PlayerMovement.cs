using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    public Animator animator;

	public Transform cameraTransform;
    private Vector3 moveDirection;


    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;


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
		animator.SetBool("Idle", true);
		animator.SetBool("run", false);
		//Debug.Log("IsGorunded: "+ characterController.isGrounded);
        if (characterController.isGrounded)
        {

            moveDirection = new Vector3();

            Vector3 forward = transform.TransformDirection(cameraTransform.forward);
            forward *= Input.GetAxis("Vertical");

            Vector3 right = transform.TransformDirection(cameraTransform.right);
            right *= Input.GetAxis("Horizontal");

            moveDirection += forward;
            moveDirection += right;
            moveDirection *= speed;

			//TODO Change character controller to use forces
			if(moveDirection.sqrMagnitude > 1)
			{
				animator.SetBool("Idle", false);
				animator.SetBool("run", true);
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
            forward *= Input.GetAxis("Vertical") * speed;

            Vector3 right = transform.TransformDirection(Vector3.right);
            right *= Input.GetAxis("Horizontal") * speed;

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
