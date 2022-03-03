using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{

    // private Camera _cam;
    private Transform _transform;
    Vector3 offset;
    private Camera _cam;
    private Vector3 originalPos;
    private bool moveup = false; 
    private string destinationTag = "DropArea";
    private bool canMove = true;

    // // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _transform = GetComponent<Transform>();
        originalPos = _transform.position;
    }


    // // Update is called once per frame
    void Update()
    {
        if(canMove){
            if(moveup && _transform.position.y != 0){
                Vector3 newPos = new Vector3(_transform.position.x, originalPos.y, _transform.position.z);
                _transform.position = Vector3.Lerp(_transform.position, newPos, Time.deltaTime * 18F);
            }
            else{
                moveup = false;
            }

        };
    
    }

    private void OnMouseDown(){
        if(canMove){
            offset = _transform.position - MouseWorldPosition();
            moveup = false;
        };
    }

    private void OnMouseDrag(){
        if(canMove){
            Vector3 newPos = MouseWorldPosition() + offset;
            if(newPos.y > (float)2.5){
            newPos.y = (float)2.5;
            }
            if(newPos.y < (float) 0){
                newPos.y = (float) 0;
            }
            _transform.position = newPos;

        };
    }

    private void OnMouseUp(){
        if(canMove){
            var rayOrigin = _cam.transform.position;
            var rayDirection = MouseWorldPosition() - _cam.transform.position;
            if(_transform.position.y == (float)2.5){
                rayDirection = _transform.position - _cam.transform.position;
            }
            RaycastHit hitInfo;
            if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo)){
                if(hitInfo.transform.tag == destinationTag){
                    _transform.position = hitInfo.transform.position;
                }
                else{
                    moveup = true;
                }
            }
        };

    }

    private void OnTriggerEnter(Collider col){
        if(canMove){
            Transform target = col.gameObject.transform;
            float diff_x = Mathf.Abs(_transform.position.x - target.position.x);
            float diff_z= Mathf.Abs(_transform.position.z - target.position.z);
            if(col.gameObject.tag == destinationTag && diff_x < 1.3f && diff_z < 1.3f){
                _transform.position = col.gameObject.transform.position;
                col.gameObject.tag = "CoverArea";
                _transform.tag = "UnDraggable";
                canMove = false;
            };
        };
    }

    Vector3 MouseWorldPosition(){
        return _cam.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y,
                Mathf.Abs(_cam.transform.position.z)
            )
        );
    }


}
