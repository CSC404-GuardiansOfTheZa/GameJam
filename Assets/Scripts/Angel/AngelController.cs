using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelController : MonoBehaviour {

    public float smoothTime = 5f;
    public float rotateSpeed = 10f;

    private Camera _cam;
    private Vector3 velocity = Vector3.zero;

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
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Lerp(transform.eulerAngles.y, velocity.x < 0 ? 90 : 270, rotateSpeed * Time.deltaTime), 0);
    }

}
