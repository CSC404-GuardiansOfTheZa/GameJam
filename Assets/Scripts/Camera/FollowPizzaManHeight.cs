using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPizzaManHeight : MonoBehaviour {
    [SerializeField] private PizzaMan pizzaMan;
    [SerializeField] private int panFrames = 6; // number of frames to pan to a new height;

    private bool wasGrounded = true;
    private float startingHeight;

    private void Start() {
        this.startingHeight = transform.position.y;
    }
    
    private void Update() {
        if (!this.wasGrounded && this.pizzaMan.IsGrounded) {
            // Just landed on the ground
            StartCoroutine(GoToPizzaManHeight(this.pizzaMan.transform.position.y + this.startingHeight));
        }
        
        this.wasGrounded = pizzaMan.IsGrounded;
    }

    IEnumerator GoToPizzaManHeight(float newHeight) {
        for (int i = 0; i < this.panFrames; i++) {
            Vector3 pos = transform.position;
            transform.position = new Vector3(
                pos.x,
                (newHeight - pos.y)/2,
                pos.z
            );
            yield return null;
        }
        
        Vector3 pos2 = transform.position;
        transform.position = new Vector3(
            pos2.x,
            newHeight,
            pos2.z
        );
    }
    
}
