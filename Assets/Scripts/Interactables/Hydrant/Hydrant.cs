using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrant : MonoBehaviour, IInteractable {
	[SerializeField] private WaterSpout spout;

	public void Start() {
		if (this.spout == null) {
			Debug.LogError("Error! spout is not set in Hydrant object!");
			Destroy(this);
		}
	}

	public void Trigger() {
		this.spout.ToggleSpout();
	}
}
