using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelController : MonoBehaviour {

    [SerializeField] private float smoothTime = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [Header("Leaning")]
    [SerializeField] private float speedForMaxLean = 50f; // When abs(velocity.x) reaches speedForMaxLean, the angel should then reach the max lean angle (specified below)
    [SerializeField] private float maxLeanAngle = 80f; // Angel can not lean more than maxLeanAngle degrees at full speed

    private Camera cam;
    private Vector3 velocity = Vector3.zero;
    
    private float lean = 0.0f;

    void Start() {
        this.cam = Camera.main;
    }

    void Update() {
        Vector3 target = this.cam.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Mathf.Abs(this.cam.transform.position.z) - 5
            )
        );

        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime * Time.deltaTime);
        transform.eulerAngles = new Vector3(
            -Mathf.Abs(this.velocity.x) / this.speedForMaxLean * this.maxLeanAngle,
            Mathf.Lerp(transform.eulerAngles.y, velocity.x < 0 ? 90 : 270, rotateSpeed * Time.deltaTime), 
            0);
        
    }

}
