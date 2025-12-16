using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour {
	#region Variables

	//* Refs
	[SerializeField] private GameObject highlighter;
	[SerializeField] private GameObject gridSystemObject;

	private InputAction openBuildMode;
	private InputAction click;

	//* States
	private bool       inBuildMode;
	private GridSystem gridSystem;
	private string     placingBuilding = "Conveyor";

	private Dictionary<string, List<string>> allowedPlacements = new Dictionary<string, List<string>>();

	#endregion

	#region Unity Functions

	private void Start() {
		gridSystem = gridSystemObject.GetComponent<GridSystem>();
		openBuildMode = InputSystem.actions["OpenBuildMode"];
		click = InputSystem.actions["Click"];
		
		var conveyorAllowedPlacements = new List<string>();
		conveyorAllowedPlacements.Add("Grass");

		allowedPlacements.Add("Conveyor", conveyorAllowedPlacements);
	}

	private void Update() {
		CheckPlayerInputs();
	}

	#endregion

	#region Custom Functions

	private void CheckPlayerInputs() {
		print(inBuildMode);
		if (openBuildMode.triggered) {
			switch (inBuildMode) {
				case true:
					inBuildMode                                        = false;
					highlighter.GetComponent<SpriteRenderer>().enabled = false;
					break;
				case false:
					inBuildMode                                        = true;
					highlighter.GetComponent<SpriteRenderer>().enabled = true;
					break;
			}
		}

		if (!inBuildMode) return;

		highlighter.GetComponent<SpriteRenderer>().color =
			allowedPlacements[placingBuilding].Contains(gridSystem.lookingAtType) ? Color.green : Color.red;

		if (click.triggered) {
			PlaceBuilding();
		}
	}

	private void PlaceBuilding() {
	}

	#endregion
}