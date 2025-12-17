using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour {
	#region Variables

	//* Refs
	[SerializeField] private GameObject buildModeUI;
	[SerializeField] private GameObject conveyor;
	[SerializeField] private Sprite     conveyorSprite;
	[SerializeField] private GameObject highlighter;
	[SerializeField] private Sprite     highlighterSprite;
	[SerializeField] private GameObject gridSystemObject;

	private InputAction openBuildMode;
	private InputAction click;
	private InputAction rotate;


	//* States
	private string     buildModeTool = "Place";
	private bool       inBuildMode;
	private GridSystem gridSystem;
	private string     placingBuilding = "Conveyor";

	private Dictionary<Vector2, GameObject>  buildingsOnMap    = new Dictionary<Vector2, GameObject>();
	private Dictionary<string, List<string>> allowedPlacements = new Dictionary<string, List<string>>();
	private Dictionary<string, GameObject>   buildings         = new Dictionary<string, GameObject>();
	private Dictionary<string, Sprite>       buildingSprites   = new Dictionary<string, Sprite>();

	#endregion

	#region Unity Functions

	private void Start() {
		gridSystem = gridSystemObject.GetComponent<GridSystem>();

		openBuildMode = InputSystem.actions["OpenBuildMode"];
		click         = InputSystem.actions["Click"];
		rotate        = InputSystem.actions["Rotate"];

		buildingSprites.Add("Conveyor", conveyorSprite);
		buildings.Add("Conveyor", conveyor);

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
		if (openBuildMode.triggered) {
			switch (inBuildMode) {
				case true:
					inBuildMode                                        = false;
					buildModeUI.SetActive(false);
					highlighter.GetComponent<SpriteRenderer>().enabled = false;
					break;
				case false:
					inBuildMode = true;
					buildModeUI.SetActive(true);
					break;
			}
		}

		if (!inBuildMode) return;

		switch (buildModeTool) {
			case "Place":
				highlighter.GetComponent<SpriteRenderer>().enabled = true;
				highlighter.GetComponent<SpriteRenderer>().sprite  = buildingSprites[placingBuilding];
				break;
			case "Remove":
				highlighter.GetComponent<SpriteRenderer>().sprite  = highlighterSprite;
				highlighter.GetComponent<SpriteRenderer>().color   = new Color(1f, 1f, 1f, 0.9f);
				highlighter.GetComponent<SpriteRenderer>().enabled = true;
				break;
		}

		if (rotate.WasPressedThisFrame()) {
			highlighter.transform.Rotate(Vector3.forward, -90f);
		}

		highlighter.GetComponent<SpriteRenderer>().color =
			allowedPlacements[placingBuilding].Contains(gridSystem.lookingAtType)
				? new Color(0f, 1f, 0f, 0.9f)
				: new Color(1f, 0f, 0f, 0.9f);

		if (click.IsPressed()) {
			switch (buildModeTool) {
				case "Place":
					PlaceBuilding();
					break;
				case "Remove":
					DestroyBuilding();
					break;
			}
		}
	}

	private void PlaceBuilding() {
		if (buildingsOnMap.ContainsKey(gridSystem.lookingAtTile) ||
		    !allowedPlacements[placingBuilding].Contains(gridSystem.lookingAtType)) return;

		var placedBuilding = Instantiate(buildings[placingBuilding], highlighter.transform.position,
		                                 highlighter.transform.rotation);
		buildingsOnMap.Add(gridSystem.lookingAtTile, placedBuilding);
	}

	private void DestroyBuilding() {
		if (!buildingsOnMap.ContainsKey(gridSystem.lookingAtTile)) return;

		Destroy(buildingsOnMap[gridSystem.lookingAtTile]);
		buildingsOnMap.Remove(gridSystem.lookingAtTile);
	}

	#endregion

	#region Event Handlers

	public void SelectPlaceMode() {
		buildModeTool = "Place";
	}

	public void SelectRemoveMode() {
		buildModeTool = "Remove";
	}

	#endregion
}