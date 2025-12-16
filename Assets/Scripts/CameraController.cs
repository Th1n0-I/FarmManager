using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour {
	//* Settings
	[SerializeField] private float zoomIntensity;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float maxZoom;
	[SerializeField] private float minZoom;

	//* Refs
	[SerializeField] private Camera camera;

	private InputAction scroll, mouseDelta, middleMouseButton;

	//* States
	private bool isHoldingMiddleMouseButton;
	
	#region Unity Methods

	private void Start() {
		Cursor.visible = true;
		
		scroll            = InputSystem.actions["ScrollWheel"];
		mouseDelta        = InputSystem.actions["MouseDelta"];
		middleMouseButton = InputSystem.actions["MiddleMouseButton"];
	}

	private void Update() {
		CheckPlayerInputs();
	}

	#endregion

	#region Custom Methods

	private void CheckPlayerInputs() {
		camera.orthographicSize -= scroll.ReadValue<Vector2>().y * zoomIntensity;

		switch (camera.orthographicSize) {
			case var value when value < minZoom:
				camera.orthographicSize = minZoom;
				break;
			case var value when value > maxZoom:
				camera.orthographicSize = maxZoom;
				break;
		}

		if (!middleMouseButton.IsPressed()) return;
		gameObject.transform.position -= new Vector3(mouseDelta.ReadValue<Vector2>().x * moveSpeed * camera.orthographicSize, mouseDelta.ReadValue<Vector2>().y * moveSpeed * camera.orthographicSize, 0.0f);
	}

	#endregion
}