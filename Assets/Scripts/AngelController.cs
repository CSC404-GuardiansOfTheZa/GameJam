using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelController : MonoBehaviour {

    private Camera _cam;

    public float smoothTime = 5f;
    private Vector3 velocity = Vector3.zero;
    public float rotateSpeed = 10f;

    public Transform model;

    void Start() {
        _cam = Camera.main;
    }

    void Update() {

        Vector3 target = _cam.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Mathf.Abs(_cam.transform.position.z) - 5
            )
        );


        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime * Time.deltaTime);

        if (velocity.x < 0) {
            model.eulerAngles = new Vector3(model.eulerAngles.x, Mathf.Lerp(model.eulerAngles.y, 90, rotateSpeed * Time.deltaTime), 0);
        } else {
            model.eulerAngles = new Vector3(model.eulerAngles.x, Mathf.Lerp(model.eulerAngles.y, 270, rotateSpeed * Time.deltaTime), 0);
        }




        // Vector3 targetDirection = target - transform.position;
        // float singleStep = rotateSpeed * Time.deltaTime;
        // // Rotate the forward vector towards the target direction by one step
        // Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        // // Draw a ray pointing at our target in
        // Debug.DrawRay(transform.position, newDirection, Color.red);
        // // Calculate a rotation a step closer to the target and applies rotation to this object
        // transform.rotation = Quaternion.LookRotation(new Vector3(transform.eulerAngles.x, newDirection.y, 0));

        print(velocity);
    }

}
