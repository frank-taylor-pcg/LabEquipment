using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
	[SerializeField] private float Speed = 8;
	[SerializeField] private float LookSpeedX = 2f;
	[SerializeField] private float LookSpeedY = 2f;
	
	// To prevent gimbal-locking the camera
	[SerializeField] private float LookLimit = 85f;
	
	private Camera camera;
	private CharacterController controller;
	
	private Vector3 direction = new Vector3();
	private float xRotation;
	
	// Start is called before the first frame update
  void Start()
  {
	  camera = GetComponentInChildren<Camera>();
	  controller = GetComponentInChildren<CharacterController>();
	  
	  // Locks the cursor to the middle of the screen and renders it invisible
	  Cursor.lockState = CursorLockMode.Locked;
	  Cursor.visible = false;
  }

  // Update is called once per frame
  void Update()
	{
		HandleMovementInput();
		HandleMouseLook();
		ApplyInput();
	}
  
	private void HandleMovementInput()
	{
		float y = direction.y;
		
		Vector3 forward = Speed * Input.GetAxis("Vertical") * transform.TransformDirection(Vector3.forward);
		Vector3 strafe = Speed * Input.GetAxis("Horizontal") * transform.TransformDirection(Vector3.right);
		
		// To prevent moving faster when strafing, normalize the inputs
		Vector3 moveDirection = (forward + strafe).normalized;
		
		// This is a tiny bit confusing, but we need to retain the original Y value and extract the XZ from the inputs
		// This is XY in the direction vector because I'm leveraging the normalization built into the Vector2 class
		direction = new Vector3(moveDirection.x, y, moveDirection.y);
		
	}
	
	private void HandleMouseLook()
	{
		xRotation -= Input.GetAxis("Mouse Y") * LookSpeedY;
		xRotation = Mathf.Clamp(xRotation, -LookLimit, LookLimit);
		camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
		transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * LookSpeedX, 0);
	}
	
	private void ApplyInput()
	{
		controller.Move(direction * Time.deltaTime);
	}
}
