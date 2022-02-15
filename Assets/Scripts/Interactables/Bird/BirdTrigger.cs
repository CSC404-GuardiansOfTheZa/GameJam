using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTrigger : MonoBehaviour {
	[SerializeField] private Bird bird;
	
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			this.bird.HoverOver(other.transform);
		}	
	}
}
