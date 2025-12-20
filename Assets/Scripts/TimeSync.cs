using System;
using Unity.Mathematics;
using UnityEngine;

public class TimeSync : MonoBehaviour
{
	//* Settins
	[SerializeField] public float conveyorSpeed = 1;
	
	
	//* States
	public float time;
	
	private void FixedUpdate() {
		time += Time.deltaTime; ;
	}
}
