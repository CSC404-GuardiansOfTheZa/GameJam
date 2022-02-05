using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _cursorPos;
    private Camera _cam;
   
    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _cam = GameObject.Find("Main Camera").GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 position = _cam.ScreenToWorldPoint(Input.mousePosition);
        _transform.position = _cam.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y,
                Mathf.Abs(_cam.transform.position.z) - 5
            )
        );
        
    }

    


}
