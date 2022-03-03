using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

	public ScoreManager ScoreManager;

	void Start(){
		ScoreManager = FindObjectOfType<ScoreManager>();
	}

    private void OnCollisionEnter(Collision collision){
    	if(collision.collider.tag == "Player"){
    		Destroy(this.gameObject);
    		ScoreManager.UpdateScore();
    	}
    }
}
