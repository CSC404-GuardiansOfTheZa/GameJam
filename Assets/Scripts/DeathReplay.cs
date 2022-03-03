using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathReplay : MonoBehaviour
{

	private Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
    }

   	private void OnCollisionEnter(Collision collision){
    	if(collision.collider.tag == "Player" && _transform.tag != "CoverArea"){
    		Debug.Log("collide with dead object");
    		SceneManager.LoadScene(5);
    	}
    }
}
