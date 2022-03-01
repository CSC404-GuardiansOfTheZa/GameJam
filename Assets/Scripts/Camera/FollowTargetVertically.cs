using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowTargetVertically : MonoBehaviour {
	[SerializeField] private PizzaMan pizzaMan;
	// a value of 0.5 will be halfway from the center to the edges; i.e. 0.25 and 0.75 from the bottom.
	[Range(0, 1)] [SerializeField] private float innerBoundaryFromCenter = 0.75f;
	[Range(0, 1)] [SerializeField] private float outerBoundaryFromCenter = 0.85f;
	[SerializeField] private float smoothTime = 0.5f;
	
	private Camera cam;
	private float targetYRelative; // Tracks the y-position of the target relative to center screen, from 0 (at center) to 1 (top/bottom)
	private Vector3 velocity = Vector3.zero;  // Velocity of the camera
	private bool justGrounded = false;

	public void Start() {
		this.cam = this.GetComponent<Camera>();
		this.pizzaMan.onGrounded += this.OnTargetGrounded;
	}

	public void Update() {
		Vector3 pizzaPos = this.pizzaMan.transform.position;
		this.targetYRelative = Mathf.Abs((this.cam.WorldToViewportPoint(pizzaPos).y * 2) - 1);

		if (this.targetYRelative > this.outerBoundaryFromCenter) {
			SmoothDampUntilTargetWithinBounds(pizzaPos.y);
		} else if (this.justGrounded) {
			if (this.targetYRelative > this.innerBoundaryFromCenter) {
				SmoothDampUntilTargetWithinBounds(pizzaPos.y);
			} else {
				this.justGrounded = false;
			}
		}
	}

	public void OnTargetGrounded() {
		this.justGrounded = true;
	}

	private void SmoothDampUntilTargetWithinBounds(float targetYPosition) {
		Vector3 cameraPosition = transform.position;
		Vector3 cameraTargetPosition = new Vector3(cameraPosition.x, targetYPosition, cameraPosition.z);
		transform.position = Vector3.SmoothDamp(
			cameraPosition, 
			cameraTargetPosition, 
			ref this.velocity, 
			this.smoothTime
		);
	}

	private void OnDrawGizmosSelected() {
		this.cam = this.GetComponent<Camera>();
		Vector3 pizzaPos = this.pizzaMan.transform.position;
		float z = Mathf.Abs(transform.position.z - pizzaPos.z);
		
		// Draw outline of camera viewport, in blue
		Vector3 botLeft = this.cam.ViewportToWorldPoint(new Vector3(0, 0, z));
		Vector3 topLeft = this.cam.ViewportToWorldPoint(new Vector3(0, 1, z));
		Vector3 botRight = this.cam.ViewportToWorldPoint(new Vector3(1, 0, z));
		Vector3 topRight = this.cam.ViewportToWorldPoint(new Vector3(1, 1, z));
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(botLeft, topLeft);
		Gizmos.DrawLine(topLeft, topRight);
		Gizmos.DrawLine(topRight, botRight);
		Gizmos.DrawLine(botRight, botLeft);

		// Draw outer boundaries, in red
		Vector3 topOuterBoundaryLeft = this.cam.ViewportToWorldPoint(new Vector3(0, 0.5f+this.outerBoundaryFromCenter/2.0f, z));
		Vector3 topOuterBoundaryRight = this.cam.ViewportToWorldPoint(new Vector3(1, 0.5f+this.outerBoundaryFromCenter/2.0f, z));
		Vector3 botOuterBoundaryLeft = this.cam.ViewportToWorldPoint(new Vector3(0, 0.5f-this.outerBoundaryFromCenter/2.0f, z));
		Vector3 botOuterBoundaryRight = this.cam.ViewportToWorldPoint(new Vector3(1, 0.5f-this.outerBoundaryFromCenter/2.0f, z));
		Gizmos.color = Color.red;
		Gizmos.DrawLine(topOuterBoundaryLeft, topOuterBoundaryRight);
		Gizmos.DrawLine(botOuterBoundaryLeft, botOuterBoundaryRight);
		
		// Draw inner boundaries, in yellow
		Vector3 topInnerBoundaryLeft = this.cam.ViewportToWorldPoint(new Vector3(0, 0.5f+this.innerBoundaryFromCenter/2.0f, z));
		Vector3 topInnerBoundaryRight = this.cam.ViewportToWorldPoint(new Vector3(1, 0.5f+this.innerBoundaryFromCenter/2.0f, z));
		Vector3 botInnerBoundaryLeft = this.cam.ViewportToWorldPoint(new Vector3(0, 0.5f-this.innerBoundaryFromCenter/2.0f, z));
		Vector3 botInnerBoundaryRight = this.cam.ViewportToWorldPoint(new Vector3(1, 0.5f-this.innerBoundaryFromCenter/2.0f, z));
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(topInnerBoundaryLeft, topInnerBoundaryRight);
		Gizmos.DrawLine(botInnerBoundaryLeft, botInnerBoundaryRight);

		// draw line from pizza guy to center of screen
		Gizmos.color = Color.cyan;
		Vector3 center = this.cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, z));
		Gizmos.DrawLine(center, pizzaPos);
	}
}
