using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 1;
    //public Transform orbitTransform;
    //public float orbitSpeed = 1;
    public float smoothSpeed = 10f;
    public float distanceScale = 1;
    public float heightScale = 1;
    public Transform followTransform;
    public Transform fpsTransform;
	//public Transform playerHead;
	public Transform playerTransform;

	//public GameObject crosshair;
    
    enum Variant { ThirdPerson, FirstPerson, Orbit };

    private Variant cameraState;

    private class CameraState
    {
        public float yaw;
        public float pitch;
        public float roll;
        public float x;
        public float y;
        public float z;

        

        public void SetFromTransform(Transform t)
        {
            SetRotationFromTransform(t);
            SetPositionFromTransform(t);
        }

        public void SetPositionFromTransform(Transform t)
        {
            x = t.position.x;
            y = t.position.y;
            z = t.position.z;
        }

        public void SetRotationFromTransform(Transform t)
        {
            pitch = t.eulerAngles.x;
            yaw = t.eulerAngles.y;
            roll = t.eulerAngles.z;
        }

        public void Translate(Vector3 translation)
        {
            Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

            x += rotatedTranslation.x;
            y += rotatedTranslation.y;
            z += rotatedTranslation.z;
        }


        public void UpdateTransform(Transform t)
        {
            Math.Round(yaw, 2);
            Math.Round(pitch, 2);
            Math.Round(roll, 2);
            Math.Round(x, 2);
            Math.Round(y, 1);
            Math.Round(z, 2);

            t.eulerAngles = new Vector3(pitch, yaw, roll);
            t.position = new Vector3(x, y, z);
        }
    }

    CameraState targetCameraState = new CameraState();

    void Start()
    {
        // Hide and lock cursor to screen
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;


        targetCameraState.SetFromTransform(followTransform);
        targetCameraState.UpdateTransform(transform);
        cameraState = Variant.ThirdPerson;

    }

    //void Update()
    //{
        
    //    handleCameraStateInput();
       
    //}

    private void LateUpdate()
    {
        switch (cameraState)
        {

			case Variant.ThirdPerson:
				// Handle mouse look-around
				Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * -1);
				mouseMovement *= mouseSensitivity;

				Vector3 smoothPosition = Vector3.Lerp(transform.position, followTransform.position, smoothSpeed * Time.deltaTime);

				//TODO Could probably keep this and have mouse movement alter the character controller rotation
                //targetCameraState.SetRotationFromTransform(followTransform);
                targetCameraState.x = smoothPosition.x;
                targetCameraState.y = smoothPosition.y;
                targetCameraState.z = smoothPosition.z;
				
				// TODO Tweak camera controller feel (should the character move with the camera?)
				//Prevent player from rotating through themselves
				targetCameraState.pitch = Mathf.Clamp(targetCameraState.pitch + mouseMovement.y, -18, 36);
				targetCameraState.yaw += mouseMovement.x;
				
				
				
				break;

            case Variant.FirstPerson:
                mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * -1);
                mouseMovement *= mouseSensitivity;

                targetCameraState.SetPositionFromTransform(fpsTransform);

                targetCameraState.yaw += mouseMovement.x;
                targetCameraState.pitch += mouseMovement.y;

                //Prevent player from rotating through themselves
                Mathf.Clamp(targetCameraState.pitch, -80, 36);
                playerTransform.eulerAngles = new Vector3(playerTransform.rotation.eulerAngles.x, targetCameraState.yaw, playerTransform.rotation.eulerAngles.z);

                break;
        }

        // Set transform to targetCameraState
        targetCameraState.UpdateTransform(transform);
    }

	// Change camera state 
	// I = First Person Camera
	// O = Orbit Camera
	// P = 3rd Person Camera
	//private void handleCameraStateInput()
	//{
		//    if (Input.GetKeyDown(KeyCode.I))
		//    {
		//        crosshair.SetActive(true);
		//        cameraState = Variant.FirstPerson;
		//        //Debug.Log("FPS");

		//        targetCameraState.SetFromTransform(fpsTransform);
		//        targetCameraState.UpdateTransform(transform);
		//    }
		//    else if (Input.GetKeyDown(KeyCode.O))
		//    {
		//        crosshair.SetActive(false);

		//        cameraState = Variant.Orbit;
		//        Debug.Log("Orbit");
		//        targetCameraState.SetFromTransform(orbitTransform);
		//        targetCameraState.UpdateTransform(transform);

		//    }
		//    else if (Input.GetKeyDown(KeyCode.P))
		//    {
		//        crosshair.SetActive(false);

		//        cameraState = Variant.ThirdPerson;
		//        Debug.Log("Thrid person");

		//        targetCameraState.SetFromTransform(followTransform);
		//        targetCameraState.UpdateTransform(transform);

		//    }

		//if (cameraState != Variant.FirstPerson)
		//{
		//	// Camera down/up
		//	if (Input.GetKeyDown(KeyCode.Q))
		//	{
		//		orbitTransform.position += Vector3.down*heightScale;
		//		followTransform.position += Vector3.down*heightScale;
		//	}
		//	else if (Input.GetKeyDown(KeyCode.E))
		//	{
		//		orbitTransform.position += Vector3.up*heightScale;
		//		followTransform.position += Vector3.up*heightScale;

		//	}
		//}

	//}

	private void OnGUI()
    {
    }

    //private void Orbit()
    //{
    //    orbitTransform.RotateAround(playerHead.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
    //}
}



