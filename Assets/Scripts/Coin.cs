using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
	[SerializeField] private int pointValue = 500;

    private void OnCollisionEnter(Collision collision) {
	    if (!collision.collider.CompareTag("Player")) return;
	    Destroy(this.gameObject);
	    ScoreManager.Instance.AddToScore(this.pointValue);
    }
}
