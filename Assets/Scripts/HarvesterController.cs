using System;
using UnityEngine;

public class HarvesterController : MonoBehaviour {
	#region Variables

	//* Settings
	[SerializeField] private float harvestSpeed;

	//* Refs
	private GameObject       gridManager;
	private GameObject       timeSyncObject;
	private GameObject       placementManager;
	private PlacementManager placementSystem;
	private GridSystem       gridSystem;
	private TimeSync         timeSync;

	//* States
	private Vector2 placedOnTile;
	private Vector2 outputTile;
	private string  cropType;

	#endregion

	#region Unity Methods

	private void Start() {
		gridManager = GameObject.FindGameObjectWithTag("GridManager");
		gridSystem  = gridManager.GetComponent<GridSystem>();

		placementManager = GameObject.FindGameObjectWithTag("PlacementManager");
		placementSystem  = placementManager.GetComponent<PlacementManager>();

		placedOnTile = new Vector2(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
		cropType     = gridSystem.grid[placedOnTile];

	}

	#endregion
}