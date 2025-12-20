using System.Collections.Generic;
using UnityEngine;

public class ConveyorSystemManager : MonoBehaviour {
	//* Refs
	private GameObject       placementManager;
	private PlacementManager placementSystem;
	
	//* States
	private List<Dictionary<int, GameObject>> conveyorSystems = new List<Dictionary<int, GameObject>>();

	#region Custom Functions

	public void NewBuildingPlaced(GameObject building, Vector2 tile, Quaternion rotation) {
		var outputTile = new Vector2(0, 0);
		var inputTile  = new Vector2(0, 0);
		switch (rotation.z) {
			case 0:
				outputTile = tile + new Vector2(-1, 0);
				inputTile  = tile + new Vector2(1,  0);
				break;
			case 90:
				outputTile = tile + new Vector2(0, -1);
				inputTile  = tile + new Vector2(0, 1);
				break;
			case 180:
				outputTile = tile + new Vector2(1,  0);
				inputTile  = tile + new Vector2(-1, 0);
				break;
			case 270:
				outputTile = tile + new Vector2(0, 1);
				inputTile  = tile + new Vector2(0, -1);
				break;
		}

		if (!(placementSystem.BuildingsOnMap.ContainsKey(outputTile) ||
		      placementSystem.BuildingsOnMap.ContainsKey(inputTile))) return;

		if (placementSystem.BuildingsOnMap.ContainsKey(outputTile) && placementSystem.BuildingsOnMap[outputTile]
			    .GetComponent<ConveyorManager>().CheckIfAvailable()) {
			
		}
	}

	#endregion
}