using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : AnimationInteractable {

    [SerializeField]
    private float bounceStrength = 5.0f;



    private void OnCollisionEnter(Collision other) {
        if (!this.isActivated) return;
        if (!other.transform.CompareTag("Player")) return;
        print("Player bounce on umbrella!");
        
        Rigidbody rigidbody = other.gameObject.GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.up * bounceStrength);
    }

}
