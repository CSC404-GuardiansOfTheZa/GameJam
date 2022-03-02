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

	public override void Trigger() {
		// TODO:
		base.Trigger();
		this.spout.ToggleSpout();
	}

	protected override void OnActivate() {
		throw new NotImplementedException();
	}

	protected override void OnDeactivate() {
		throw new NotImplementedException();
	}
}
