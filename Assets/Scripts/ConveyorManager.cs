using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
	//* Refs
	private GameObject placementManager, gridManager;
	private PlacementManager placementSystem;
	private GridSystem gridSystem;
	
	//* States 
	private Vector2 placedOnTile;
	private Vector2 outputTile;

	private bool occupied;

	private void Start() {
		gridManager = GameObject.FindGameObjectWithTag("GridManager");
		placementManager = GameObject.FindGameObjectWithTag("PlacementManager");
		gridSystem = gridManager.GetComponent<GridSystem>();
		placementSystem = placementManager.GetComponent<PlacementManager>();
		
		placedOnTile = new Vector2(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
		
		switch (transform.rotation.z % 360) {
			case 0:   outputTile = placedOnTile + new Vector2(-1, 0); break;
			case 90:  outputTile = placedOnTile + new Vector2(0,  -1); break;
			case 180: outputTile = placedOnTile + new Vector2(1,  0); break;
			case 270: outputTile = placedOnTile + new Vector2(0,  1); break;
		}
	}
	
	
	public bool CheckIfAvailable() {
		if (!placementSystem.buildingsOnMap.ContainsKey(outputTile)) {
			return !occupied;
		} else {
			return placementSystem.buildingsOnMap[outputTile].GetComponent<ConveyorManager>().CheckIfAvailable();
		}
	}
}
