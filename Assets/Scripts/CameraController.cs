using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	//* Settings
	[SerializeField] private float zoomIntensity;
	[SerializeField] private float moveSpeed;
	//* Refs
	private InputAction scroll, move;
	#region Unity Methods

	private void Start() {
		scroll = InputSystem.actions["ScrollWheel"];
	}
    private void Update() {
	    CheckPlayerInputs();
    }
	#endregion
	
	#region Custom Methods
    private void CheckPlayerInputs() {
	    gameObject.GetComponent<Camera>().orthographicSize -= scroll.ReadValue<Vector2>().y * zoomIntensity;
    }
    #endregion
}
