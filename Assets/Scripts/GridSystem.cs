using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour {
	//* Refs
	[SerializeField] private Tilemap tilemap;
	[SerializeField] private Sprite  water;
	[SerializeField] private Sprite  wheat;
	[SerializeField] private Sprite  grass;

	//* Settings
	[SerializeField] private int mapSize;
	[SerializeField] private int gridSize = 1;

	//* Information
	private Dictionary<Vector2, string> grid = new Dictionary<Vector2, string>();

	//*States
	private InputAction uiMousePosition;

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
		Debug.Log(gridPosition);
		Debug.Log(mousePosition);
		Debug.Log(grid.ContainsKey(gridPosition) ? grid[gridPosition] : "No grid position");
		Debug.Log(tilemap.GetSprite(new Vector3Int((int)mousePosition.x, (int)mousePosition.y, 0)));
	}

	#endregion
}