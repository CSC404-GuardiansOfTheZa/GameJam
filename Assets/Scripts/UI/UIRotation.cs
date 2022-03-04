using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotation : MonoBehaviour {
	[SerializeField] private Vector3 rotationSpeeds = new Vector3(0f, 0f, 60f); // degrees per second
	[SerializeField] private bool clockwise = true;

	public void Update() {
		transform.Rotate(this.rotationSpeeds * (Time.deltaTime * (this.clockwise ? -1 : 1)));
	}
}
