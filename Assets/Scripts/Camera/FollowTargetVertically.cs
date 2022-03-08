using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowTargetVertically : MonoBehaviour {
	[SerializeField] private PizzaMan pizzaMan;
	// a value of 0.5 will be halfway from the center to the edges; i.e. 0.25 and 0.75 from the bottom.
	[Range(0, 1f)] [SerializeField] private float innerBoundaryTop = 0.75f;
	[Range(0, 1f)] [SerializeField] private float innerBoundaryBottom = 0.75f;
	[Range(0, 1.5f)] [SerializeField] private float outerBoundaryTop = 0.85f;
	[Range(0, 1.5f)] [SerializeField] private float outerBoundaryBottom = 0.85f;
	[SerializeField] private float smoothTime = 0.5f;
	[SerializeField] private float marginOfError = 0.2f; // how close to the target positions the camera can go.
	
	private Camera cam;
	private float targetYRelative; // Tracks the y-position of the target relative to center screen, from 0 (at center) to 1 (top/bottom)
	private Vector3 velocity = Vector3.zero;  // Velocity of the camera
	private bool justGrounded = false;
	private bool mutex;
	private float baseCameraHeight;

	public void Start() {
		this.cam = this.GetComponent<Camera>();
		this.baseCameraHeight = transform.position.y;
		this.pizzaMan.onGrounded += this.OnTargetGrounded;
		LevelManager.Instance.OnLevelStart += this.OnLevelStart;
	}

	public void Update() {
		Vector3 pizzaPos = this.pizzaMan.transform.position;
		Vector3 cameraPos = this.transform.position;
		this.targetYRelative = (this.cam.WorldToViewportPoint(pizzaPos).y * 2) - 1;

		if (this.mutex) {}  // i.e. do nothing if a mutex is already set
		else if (this.targetYRelative > this.outerBoundaryTop || this.targetYRelative < -this.outerBoundaryBottom) {
			transform.position = Vector3.SmoothDamp(
				cameraPos,
				new Vector3(cameraPos.x, pizzaPos.y + this.baseCameraHeight, cameraPos.z), 
				ref this.velocity, 
				this.smoothTime/2 // double speed for panning 
			);
		} else if (this.justGrounded) {
			if (this.targetYRelative > this.innerBoundaryTop || this.targetYRelative < -this.outerBoundaryBottom) {
				StartCoroutine(SmoothDampUntilTarget(pizzaPos.y + this.baseCameraHeight));
			} 
			this.justGrounded = false;
		}
	}

	public void OnTargetGrounded() {
		this.justGrounded = true;
	}

	public void OnLevelStart() {
		this.baseCameraHeight = transform.position.y - this.pizzaMan.transform.position.y;
	}

	private IEnumerator SmoothDampUntilTarget(float targetYPosition) {
		if (this.mutex) yield break;
		this.mutex = true;
		
		Vector3 cameraPosition = transform.position;
		Vector3 cameraTargetPosition = new Vector3(cameraPosition.x, targetYPosition, cameraPosition.z);

		while (Mathf.Abs(transform.position.y - targetYPosition) > this.marginOfError) {
			transform.position = Vector3.SmoothDamp(
				transform.position,
				cameraTargetPosition, 
				ref this.velocity, 
				this.smoothTime
			);
			yield return null;
		}

		this.mutex = false;
	}
	
	private void OnDrawGizmos() {
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
		Vector3 topOuterBoundaryLeft = this.cam.ViewportToWorldPoint(new Vector3(0, 0.5f+this.outerBoundaryTop/2.0f, z));
		Vector3 topOuterBoundaryRight = this.cam.ViewportToWorldPoint(new Vector3(1, 0.5f+this.outerBoundaryTop/2.0f, z));
		Vector3 botOuterBoundaryLeft = this.cam.ViewportToWorldPoint(new Vector3(0, 0.5f-this.outerBoundaryBottom/2.0f, z));
		Vector3 botOuterBoundaryRight = this.cam.ViewportToWorldPoint(new Vector3(1, 0.5f-this.outerBoundaryBottom/2.0f, z));
		Gizmos.color = Color.red;
		Gizmos.DrawLine(topOuterBoundaryLeft, topOuterBoundaryRight);
		Gizmos.DrawLine(botOuterBoundaryLeft, botOuterBoundaryRight);
		
		// Draw inner boundaries, in yellow
		Vector3 topInnerBoundaryLeft = this.cam.ViewportToWorldPoint(new Vector3(0, 0.5f+this.innerBoundaryTop/2.0f, z));
		Vector3 topInnerBoundaryRight = this.cam.ViewportToWorldPoint(new Vector3(1, 0.5f+this.innerBoundaryTop/2.0f, z));
		Vector3 botInnerBoundaryLeft = this.cam.ViewportToWorldPoint(new Vector3(0, 0.5f-this.innerBoundaryBottom/2.0f, z));
		Vector3 botInnerBoundaryRight = this.cam.ViewportToWorldPoint(new Vector3(1, 0.5f-this.innerBoundaryBottom/2.0f, z));
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(topInnerBoundaryLeft, topInnerBoundaryRight);
		Gizmos.DrawLine(botInnerBoundaryLeft, botInnerBoundaryRight);

		// draw line from pizza guy to center of screen
		Gizmos.color = Color.cyan;
		Vector3 center = this.cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, z));
		Gizmos.DrawLine(center, pizzaPos);
		
		// draw white cross, so we know where center is
		Gizmos.color = Color.white;
		Vector3 topCenter = this.cam.ViewportToWorldPoint(new Vector3(0.5f, 1, z));
		Vector3 botCenter = this.cam.ViewportToWorldPoint(new Vector3(0.5f, 0, z));
		Gizmos.DrawLine(topCenter, botCenter);
		Vector3 leftCenter = this.cam.ViewportToWorldPoint(new Vector3(0, 0.5f, z));
		Vector3 rightCenter = this.cam.ViewportToWorldPoint(new Vector3(1, 0.5f, z));
		Gizmos.DrawLine(leftCenter, rightCenter);
	}
}
