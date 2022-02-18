using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathReplay : MonoBehaviour
{
   	private void OnCollisionEnter(Collision collision){
    	if(collision.collider.tag == "Player"){
    		Debug.Log("collide with dead object");
    		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    	}
    }
}
