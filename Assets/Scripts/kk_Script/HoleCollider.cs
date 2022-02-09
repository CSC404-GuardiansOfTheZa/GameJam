using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleCollider : MonoBehaviour
{
    [SerializeField]
    private LevelManager lvlmgmt;

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision){

    	if(collision.collider.tag == "Player"){
    		//the game end
            lvlmgmt.Lose();
    	}
    }
}
