using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public abstract class IntDisplay : MonoBehaviour {

	private TextMeshProUGUI textField;
	
	private void Start() {
		this.textField = this.GetComponent<TextMeshProUGUI>();
		this.SubscribeToEvents();
	}

	public void UpdateText(int n) {
		this.textField.SetText(n.ToString());
	}
	
	// this is abstract to ensure child classes *do* subscribe
	protected abstract void SubscribeToEvents();
}
