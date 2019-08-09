using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    public Animator animator;

    private Vector3 moveDirection;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float peen1 = 0.0f;
    public int sexy = 696969;
    public float gravity = 20.0f;
    public float peen = 0.0f;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        animator.SetBool("IsJump", false);

        if (characterController.isGrounded)
        {

            moveDirection = new Vector3();

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            forward *= Input.GetAxis("Vertical");

            Vector3 right = transform.TransformDirection(Vector3.right);
            right *= Input.GetAxis("Horizontal");

            moveDirection += forward;
            moveDirection += right;
            moveDirection *= speed;


            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                animator.SetBool("IsJump", true);
            }
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
        animator.SetFloat("Speed", moveDirection.sqrMagnitude);
    }


}
