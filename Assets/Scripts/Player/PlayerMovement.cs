using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the movement calculations as well as communication with the animation controller for the player movement
/// </summary>
public class PlayerMovement : MonoBehaviour
{
	CharacterController characterController;
	public Animator animator;
	[SerializeField] private AnimationClip jumpAnim = null; //Assigned in inspector

	public Transform cameraTransform;
	private Transform horizontalCameraTransform;

	public float speedMultiplier = 6.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;

	[Range(0, 1)]
	public float turnSpeed;

	public Vector3 debugVelocity;

	private Vector3 moveDirection;
	private float jumpAnimTime;

	private PlayerAttack playerAttack;

	void Start()
	{
		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		jumpAnimTime = jumpAnim.length;
		playerAttack = GetComponent<PlayerAttack>();

	}

	/// <summary>
	/// <list type="number">
	/// <item>
	/// <description>Handles all the movement calculations required each frame</description>
	/// </item>
	/// <item>
	/// <description>Input-axis are multiplied onto a forward vector (take from the camera horizontal) and converted to local coordinates</description>
	/// </item>
	/// <item>
	/// <description>Relative velocity is sent to the animator along with x/y speeds for use in animation blending</description>
	/// </item>
	/// <item>
	/// <description>Provided the player is moving, turns the character to face in the direction of the camera</description>
	/// </item>
	/// <item>
	/// <description>Allows for jumping when grounded, otherwise applies pseudo-gravity (fixed speed)</description>
	/// </item>
	/// </list>
	/// </summary>
	void Update()
	{
		moveDirection = new Vector3();

		Vector3 forward = cameraTransform.GetChild(1).transform.forward;
		forward.y = 0;
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
		
		if (speed > 0 || playerAttack.inMelee)
		{
			//Turn character
			Quaternion newLookRotation = Quaternion.LookRotation(cameraTransform.forward, Vector3.up);
			newLookRotation.x = 0;
			newLookRotation.z = 0;
			//transform.rotation = Quaternion.Slerp(transform.rotation, newLookRotation, Time.deltaTime * turnSpeed);
			//transform.rotation = Quaternion.Slerp(transform.rotation, newLookRotation, turnSpeed);
			//transform.rotation = Quaternion.Slerp(transform.rotation, newLookRotation, Time.deltaTime * turnSpeed);
			transform.rotation = Quaternion.Lerp(transform.rotation, newLookRotation, turnSpeed); //TODO: Decide on one of these 4 lines lines
		}

		if (characterController.isGrounded && gravity >= 0)
		{
			animator.SetFloat("velocityX", localVelocity.x);
			animator.SetFloat("velocityZ", localVelocity.z);

			if (Input.GetButtonDown("Jump"))
			{
				animator.SetTrigger("jump");
			}
			moveDirection.y = gravity * -characterController.stepOffset/Time.deltaTime;
		}
		else
		{
			moveDirection.y -= gravity * Time.deltaTime;
		}

		// Move the controller
		characterController.Move(moveDirection * Time.deltaTime);
	}

	private void OnJumpEvent()
	{
		StartCoroutine(GravityPauseForJump());
	}

	//TODO Tweak this
	IEnumerator GravityPauseForJump()
	{
		float tGrav = gravity;
		float tCCHeight = characterController.height;
		Vector3 tCCCenter = characterController.center;
		Vector3 tPlayerPosition = transform.position;

		gravity = -gravity;
		//transform.position += Vector3.up * 0.5f;
		//characterController.center = new Vector3(tCCCenter.x, 0.92f, tCCCenter.z);
		//characterController.height = 1.391f;
		//characterController.Move(Vector3.up * 1);


		Debug.Log("Jumping");

		yield return new WaitForSeconds(0.5f);

		gravity = tGrav;
		//characterController.center = tCCCenter;
		//characterController.height = tCCHeight;

		Debug.Log("End Jumping");
	}

	/// <summary>
	/// Copys local Pos, Rotation and Scale onto a transform
	/// </summary>
	/// <param name="lhs"> the new copy</param>
	/// <param name="toBeCopied"> the transform from which to copy values</param>
	private void ShallowCopyTransform(Transform lhs, Transform toBeCopied)
	{
		lhs.localPosition = toBeCopied.localPosition;
		lhs.localRotation = toBeCopied.localRotation;
		lhs.localScale    = toBeCopied.localScale;
	}
	
	private void RandomizeIdleVariant()
	{
		animator.SetFloat("idleVariant", Random.Range(0, 7));
	}

	public void RandomizeReactHitVariant()
	{
		animator.SetFloat("reachHitVariant", Random.Range(0, 5));
	}

	public void RandomizeDeathVariant()
	{
		animator.SetFloat("deathVariant", Random.Range(0, 5));

	}

}

