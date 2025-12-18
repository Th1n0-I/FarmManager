using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour {
	

	#region Variables
	//* Hashes
	private static readonly int CloseBuildMode = Animator.StringToHash("CloseBuildMode");
	private static readonly int OpenBuildMode = Animator.StringToHash("OpenBuildMode");
	
	//* Refs
	//*		Buildings
	[SerializeField] private GameObject grainMill;
	[SerializeField] private Sprite     grainMillSprite;
	[SerializeField] private GameObject conveyor;
	[SerializeField] private Sprite     conveyorSprite;
	[SerializeField] private GameObject highlighter;
	[SerializeField] private Sprite     highlighterSprite;
	//*		Other
	[SerializeField] private GameObject buildModeUI;
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
		
		//? Adds buildings sprites and gameobjects to dictionaries.
		buildingSprites.Add("Conveyor", conveyorSprite);
		buildings.Add("Conveyor", conveyor);
		
		var tempAllowedPlacements = new List<string>();
		tempAllowedPlacements.Add("Grass");
		tempAllowedPlacements.Add("Wheat");
		allowedPlacements.Add("Conveyor", tempAllowedPlacements);

		buildingSprites.Add("GrainMill", grainMillSprite);
		buildings.Add("GrainMill", grainMill);
		
		allowedPlacements.Add("GrainMill", tempAllowedPlacements);

		
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
					inBuildMode = false;
					buildModeUI.GetComponent<Animator>().SetTrigger(CloseBuildMode);
					highlighter.GetComponent<SpriteRenderer>().enabled = false;
					break;
				case false:
					inBuildMode = true;
					buildModeUI.GetComponent<Animator>().SetTrigger(OpenBuildMode);
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
			allowedPlacements[placingBuilding].Contains(gridSystem.lookingAtType) && !buildingsOnMap.ContainsKey(gridSystem.lookingAtTile)
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

	public void SelectConveyor() {
		placingBuilding = "Conveyor";
		if (buildModeTool == "place") highlighter.GetComponent<SpriteRenderer>().sprite = buildingSprites["Conveyor"]; 
	}

	public void SelectGrainMill() {
		placingBuilding = "GrainMill";
		if (buildModeTool == "place") highlighter.GetComponent<SpriteRenderer>().sprite = buildingSprites["GrainMill"]; 
	}

	#endregion
}