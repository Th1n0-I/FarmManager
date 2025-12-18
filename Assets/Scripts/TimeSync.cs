using System;
using UnityEngine;

public class TimeSync : MonoBehaviour
{
	//* States
	public float time;
	
	private void FixedUpdate() {
		time += Time.deltaTime;
	}
}
