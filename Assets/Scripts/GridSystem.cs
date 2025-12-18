using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour {
	//* Refs
	[SerializeField] private Tilemap    tilemap;
	[SerializeField] private Sprite     water, wheat, grass;
	[SerializeField] private GameObject highlight;

	//* Settings
	[SerializeField] private int mapSize, gridSize;

	//* Information
	public Dictionary<Vector2, string> grid         = new Dictionary<Vector2, string>();

	//*States
	private InputAction uiMousePosition;
	public  string      lookingAtType;
	public  Vector2     lookingAtTile;

	#region Unity Methods

	private void Start() {
		AssignTiles();

		uiMousePosition = InputSystem.actions.FindAction("look");
	}

	private void Update() {
		CheckMousePosition();
	}

	#endregion

	#region Custom Methods

	private void AssignTiles() {
		for (var x = mapSize; x >= 0; x--) {
			Debug.Log(x);
			for (var y = mapSize; y >= 0; y--) {
				Debug.Log(y);
				if (grid.ContainsKey(new Vector2(x, y))) continue;
				if (!(water || wheat || grass)) return;
				switch (tilemap.GetSprite(new Vector3Int(x, y, 0)).name) {
					case ("wheat_0"):
						grid.Add(new Vector2(x, y), "Wheat");
						break;
					case ("water_0"):
						grid.Add(new Vector2(x, y), "Water");
						break;
					case ("Grass_0"):
						grid.Add(new Vector2(x, y), "Grass");
						break;
				}
			}
		}
	}

	private void CheckMousePosition() {
		if (!Camera.main) return;

		var mousePosition = Camera.main.ScreenToWorldPoint(uiMousePosition.ReadValue<Vector2>());
		mousePosition.z = 0;

		var gridPosition = new Vector2(Mathf.FloorToInt(mousePosition.x / gridSize),
		                               Mathf.FloorToInt(mousePosition.y / gridSize));

		highlight.transform.position = gridPosition + new Vector2(0.5f, 0.5f);

		lookingAtType = grid[gridPosition];
		lookingAtTile = gridPosition;
	}

	#endregion
}