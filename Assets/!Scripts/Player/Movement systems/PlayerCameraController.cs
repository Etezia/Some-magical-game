using FishNet.Component.Transforming;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NetworkTransform))]
public class PlayerCameraController : NetworkBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float yRotationSpeed = 1f;
    [SerializeField] private float xRotationSpeed = 1f;
	[SerializeField] private float upLimitAngle = -90f;
	[SerializeField] private float downLimitAngle = 85f;

	private float currRotationX = 0f;

	[SerializeField] private GameObject playerCamera;

	[SerializeField] private PlayerInput inputSystem;
	private InputAction rotateCameraAction;

	private void Awake()
	{
		CheckForComponentReferences();

		rotateCameraAction = inputSystem.actions.FindAction("RotateCamera");

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Start() => currRotationX = 0f;

	public override void OnStartClient()
	{
		if (IsOwner)
			playerCamera.SetActive(true);
	}

	private void Update()
	{
		if (!IsOwner)
			return;

		var inputMouseVector = rotateCameraAction.ReadValue<Vector2>();

		currRotationX -= inputMouseVector.y * xRotationSpeed;
		currRotationX = Mathf.Clamp(currRotationX, upLimitAngle, downLimitAngle);
		transform.localRotation = Quaternion.Euler(currRotationX, 0f, 0f);

		playerTransform.Rotate(0f, inputMouseVector.x * yRotationSpeed, 0f);

		Debug.Log(inputMouseVector);
	}

	private void CheckForComponentReferences()
	{
		if (playerTransform == null)
			Debug.Log("Playr transform is null");
		if (inputSystem == null)
			Debug.Log("Input system is null");
		if (playerCamera == null)
			Debug.Log("Player camera is null");
	}
}
