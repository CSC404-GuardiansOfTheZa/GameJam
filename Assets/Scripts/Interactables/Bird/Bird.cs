using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelImporter;

public class Bird : MonoBehaviour, IInteractable {
	[SerializeField] private Vector3 relativeStartPos = new Vector3(-40, 50, 0); // Where to move to when beginning hover animation; relative to pizza guy's coords
	[SerializeField] private float hoverDistance = 2.5f; // # of units to hover over the ground 
	[SerializeField] private float hoverDuration = 10f; // # of seconds to hover over the pizza guy, if not shoo'd
	[SerializeField] private int framesToBeginHover = 60; // # of frames from trigger to hovering. TODO: change to seconds

	private Collider col;
	private bool isShooed;

	private void Awake() {
		this.col = this.GetComponent<Collider>();
	}

	public void HoverOver(Transform target) {
		transform.SetParent(null);
		Vector3 targetPos = target.position; 
		StartCoroutine(BeginHover(
			targetPos + this.relativeStartPos,
			targetPos + (this.hoverDistance * Vector3.up)
		));
	}

	IEnumerator BeginHover(Vector3 startPos, Vector3 vertexPt) {
		// Based on a parabola that passes through startPos
		// If exit is true, goes from startPos to vertexPt. Else, go from vertexPt to StartPos
		float a = (startPos.y - vertexPt.y) / ((startPos.x - vertexPt.x) * (startPos.x - vertexPt.x));
		
		for (int frame = 0; frame <= this.framesToBeginHover; frame++) {
			float x = Mathf.Lerp(
				startPos.x, 
				vertexPt.x, 
				(float) frame / this.framesToBeginHover
			);
			float y = (a * (x - vertexPt.x) * (x - vertexPt.x)) + vertexPt.y;  // y = a (x - h)^2 + k
			transform.position = new Vector3(x, y, vertexPt.z);
			yield return null;
		}

		StartCoroutine(this.Hover());
	}

	IEnumerator Hover() {
		yield return new WaitForSeconds(this.hoverDuration);
		if (!this.isShooed) this.Trigger();
	}

	IEnumerator EndHover(float newHeight) {
		Vector3 targetPos = transform.position + (newHeight * Vector3.up);
		for (int frame = 0; frame <= this.framesToBeginHover; frame++) {
			transform.position = Vector3.Lerp(
				transform.position, 
				targetPos, 
				Mathf.Pow((float) frame / this.framesToBeginHover, 2)
			);
			yield return null;
		}	
		this.gameObject.SetActive(false);
	}

	public void Trigger() {
		// Shoo away the Bird
		// Disable colider 
		this.isShooed = true;
		this.col.enabled = false;
		
		StartCoroutine(EndHover(10f)); // TODO: turn this magic number into a SerializeField
	}

	private void Update() {
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.B)) this.Trigger();
		#endif
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
