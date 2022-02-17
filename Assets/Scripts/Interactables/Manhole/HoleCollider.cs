using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleCollider : MonoBehaviour
{
    private Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider col){
    	if(col.gameObject.tag == "Player" && _transform.tag == "DropArea"){
    		col.gameObject.GetComponent<Collider>().enabled = false;
    	}
       
    }
}
