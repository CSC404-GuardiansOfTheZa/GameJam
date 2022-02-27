using System.Collections;
using UnityEngine;

public class Window : MonoBehaviour, IInteractable {
	[SerializeField] private float secondsToOpen = 2.0f;
	[Header("Rotation")]
	[SerializeField] private Transform pivot;
	[SerializeField] private float closedXRotation = 0.0f;
	[SerializeField] private float openXRotation = 90.0f;
	[Header("Scale")] 
	[SerializeField] private float scaleMultiplierWhenOpen = 2.5f;
	[Header("Start the window open")]
	[OnChangedCall("SetStartWindow")]
	[SerializeField] private bool startOpen;

	private bool isOpen;
	private bool openingMutex = false; // must wait for window to fully open before closing again, and vice-versa
	
	public void Start() {
		this.isOpen = this.startOpen;
		this.StartCoroutine(this.SetWindow());
	}
	
	public void Trigger() {
		Debug.Log("Window should be set now!");
		this.isOpen = !this.isOpen;
		StartCoroutine(this.SetWindow());
	}

	public void SetStartWindow() {
		float xRotation = !this.startOpen ? this.openXRotation : this.closedXRotation;
		this.pivot.eulerAngles = new Vector3(xRotation, 0, 0);
	}
	
	IEnumerator SetWindow() {
		if (this.openingMutex) yield return null;
		this.openingMutex = true;

		// if this.isOpen is true, then that means its currently closed and this coroutine will open it
		float startRot = !this.isOpen ? this.openXRotation : this.closedXRotation;
		float endRot = this.isOpen ? this.openXRotation : this.closedXRotation;

		float startScale = !this.isOpen ? this.scaleMultiplierWhenOpen : 1;
		float endScale = this.isOpen ? this.scaleMultiplierWhenOpen : 1;

		float timeElapsed = 0.0f;
		while (timeElapsed < this.secondsToOpen) {
			float t = timeElapsed / this.secondsToOpen;
			
			float xRotation = this.EasedLerp(startRot, endRot, t);
			this.pivot.eulerAngles = new Vector3(xRotation, 0, 0);

			Vector3 pivotScale = this.pivot.localScale;
			pivotScale.y = this.EasedLerp(startScale, endScale, t);
			pivot.localScale = pivotScale;

			timeElapsed += Time.deltaTime;
			yield return null;
		}

		this.pivot.eulerAngles = new Vector3(endRot, 0, 0);
		this.openingMutex = false;
	}

	private float EasedLerp(float a, float b, float t) {
		// https://easings.net/en#easeOutBack
		float newT = 1 - Mathf.Pow(1 - t, 3);
		return Mathf.Lerp(a, b, newT);
	}
}
