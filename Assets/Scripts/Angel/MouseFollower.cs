using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private Vector3 _cursorPos;
    private Camera _cam;
   
    // Start is called before the first frame update
    void Start()
    {
        _cam = GameObject.Find("Main Camera").GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _cam.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y,
                Mathf.Abs(_cam.transform.position.z) - 5
            )
        );   
    }
}
