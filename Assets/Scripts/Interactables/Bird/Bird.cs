using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Bird : MonoBehaviour, IInteractable {
	[SerializeField] private Vector3 relativeStartPos = new Vector3(-40, 50, 0); // Where to move to when beginning hover animation; relative to pizza guy's coords
	[SerializeField] private float hoverDistance = 2.5f; // # of units to hover over the ground 
	[SerializeField] private float hoverDuration = 10f; // # of seconds to hover over the pizza guy, if not shoo'd
	[SerializeField] private int framesToBeginHover = 60; // # of frames from trigger to hovering.
	
	public void HoverOver(Transform target) {
		Vector3 targetPos = target.position; 
		transform.position = targetPos + this.relativeStartPos;
		transform.SetParent(null);
		StartCoroutine(BeginHover(targetPos));
	}

	IEnumerator BeginHover(Vector3 targetPos) {
		// Based on a parabola, with vertex at (targetPos+hoverDistance), that passes thru relativeStartPos
		Vector3 vertex = targetPos + (hoverDistance * Vector3.up);
		Vector3 startPos = transform.position;
		float a = (startPos.y - vertex.y) / ((startPos.x - vertex.x) * (startPos.x - vertex.x));
		
		for (int frame = 0; frame <= this.framesToBeginHover; frame++) {
			float x = Mathf.Lerp(
				startPos.x, 
				vertex.x, 
				(float) frame / this.framesToBeginHover
			);
			float y = (a * (x - vertex.x) * (x - vertex.x)) + vertex.y;  // y = a (x - h)^2 + k
			transform.position = new Vector3(x, y, startPos.z);
			yield return null;
		}
	}

	public void Trigger() {
		// Shoo away the Bird
	}

	float easeOutBack(float x) {
		// Given x in range [0, 1], ease it according to easeOutBack
		// Code from https://easings.net/en#easeOutBack
		// TODO: doesn't work rn, may use later. if you're reading this, feel free to delete this function  -Stew
		const float A = 1.70158f;
		const float B = A + 1;
		return 1 + (A * Mathf.Pow(x-1, 2)) + (B * Mathf.Pow(x - 1, 3));
	}
}
