using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class HoleCollider : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision){
    	if(collision.collider.tag == "Player"){
    		//the game end
            LevelManager.Instance.Lose();
    	}
    }
}
