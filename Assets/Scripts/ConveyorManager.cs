using System;
using UnityEngine;

public class ConveyorManager : MonoBehaviour {
	//* Refs
	private GameObject       placementManager, gridManager, timeSyncObject;
	private PlacementManager placementSystem;
	private GridSystem       gridSystem;
	private TimeSync         timeSync;

	//* States 
	private Vector2 placedOnTile;
	private Vector2 outputTile;
	private Vector2 inputTile;

	public bool occupied;

	private void Start() {
		timeSyncObject   = GameObject.FindGameObjectWithTag("TimeSync");
		gridManager      = GameObject.FindGameObjectWithTag("GridManager");
		placementManager = GameObject.FindGameObjectWithTag("PlacementManager");
		timeSync         = timeSyncObject.GetComponent<TimeSync>();
		gridSystem       = gridManager.GetComponent<GridSystem>();
		placementSystem  = placementManager.GetComponent<PlacementManager>();

		placedOnTile = new Vector2(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
		
		Debug.Log(transform.rotation.eulerAngles.z + " " + (Math.Abs(transform.rotation.eulerAngles.z) + 360) % 360 );
		switch ((Math.Abs(transform.rotation.eulerAngles.z) + 360) % 360) {
			case 0:
				outputTile = placedOnTile + new Vector2(0, 1);
				inputTile  = placedOnTile + new Vector2(0, -1);
				break;
			case 90:
				outputTile = placedOnTile + new Vector2(-1, 0);
				inputTile  = placedOnTile + new Vector2(1,  0);
				break;
			case 180:
				outputTile = placedOnTile + new Vector2(0, -1);
				inputTile  = placedOnTile + new Vector2(0, 1);
				break;
			case 270:
				outputTile = placedOnTile + new Vector2(1,  0);
				inputTile  = placedOnTile + new Vector2(-1, 0);
				break;
		}
	}

	private void FixedUpdate() {
		if (occupied) {
			gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		} else {
			gameObject.GetComponent<SpriteRenderer>().color = Color.green;
		}


		if (timeSync.time % timeSync.conveyorSpeed < Time.fixedDeltaTime) {
			Debug.Log("1" + placedOnTile + outputTile + inputTile);
			if (!placementSystem.BuildingsOnMap.ContainsKey(outputTile)) return;
			Debug.Log("1.5");
			if (placementSystem.BuildingsOnMap.ContainsKey(inputTile)) {
				if (!placementSystem.BuildingsOnMap[inputTile].CompareTag("Conveyor")                  ||
				    placementSystem.BuildingsOnMap[inputTile].GetComponent<ConveyorManager>().occupied) return;
			}

			if (!occupied) return;
			Debug.Log("2");
			if (placementSystem.BuildingsOnMap[outputTile].GetComponent<ConveyorManager>().CheckIfAvailable()) MoveItemOnBelt();
		}
	}

	public bool CheckIfAvailable() {
		Debug.Log("Checking if available" + gameObject.name + occupied);
		if (!occupied) return true;
		Debug.Log("12");
		if (!placementSystem.BuildingsOnMap[outputTile].CompareTag("Conveyor")) return false;
		Debug.Log("2");
		Debug.Log(placementSystem.BuildingsOnMap[outputTile].GetComponent<ConveyorManager>().CheckIfAvailable());
		if (placementSystem.BuildingsOnMap[outputTile].GetComponent<ConveyorManager>().CheckIfAvailable()) {
			
			return true;
		} else return false;
	}

	private void MoveItemOnBelt() {
		occupied                                                                            = false;
		placementSystem.BuildingsOnMap[outputTile].GetComponent<ConveyorManager>().occupied = true;
	}
}