using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
	// use observer pattern and singleton pattern
	// store list of listeners, using a dictionary/hash map
	// keys are the label names, and values are delegates that are to be called every frame.
	// could be expanded to instead have an "options" class or dictionary or something, but just storing delegates will be fine for now
	private static DebugPanel _instance;
	public static DebugPanel Instance => _instance;

	[SerializeField] private GameObject debugFieldInstance;
	
	private Dictionary<string, Func<string>> listeners = new Dictionary<string, Func<String>>();
	
	private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
	}

	public void AddDebugLog(string label, Func<string> func) {
		if (this.debugFieldInstance is null) return;

		GameObject debugFieldObj = Instantiate(this.debugFieldInstance, transform) as GameObject;
		DebugField debugField = debugFieldObj.GetComponent<DebugField>();
		debugField.Init(label, func);
	}
}
