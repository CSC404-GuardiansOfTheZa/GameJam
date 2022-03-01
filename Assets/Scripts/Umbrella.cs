using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour {

    [SerializeField]
    private float bounceStrength = 5.0f;


    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        print("Player bounce on umbrella!");
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.up * bounceStrength);
    }

}
