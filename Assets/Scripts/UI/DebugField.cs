using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugField : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI labelText;
	[SerializeField] private TextMeshProUGUI debugText;

	private Func<string> debugData;
	private bool initialized;

	public void Init(string label, Func<string> func) {
		this.labelText.SetText(label);
		this.debugData = func;
		this.initialized = true;
	}
	
	public void Update() {
		if (this.initialized && this.debugData != null) {
			string data = this.debugData();
			this.debugText.SetText(data);
		}
	}
}
