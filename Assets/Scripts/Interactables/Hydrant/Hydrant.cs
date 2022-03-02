using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrant : Interactable {
	[SerializeField] private WaterSpout spout;

	public void Start() {
		if (this.spout == null) {
			Debug.LogError("Error! spout is not set in Hydrant object!");
			Destroy(this);
		}
	}

	protected override void OnActivationChange() {
		this.spout.ToggleSpout(this.IsActive);
	}
}
