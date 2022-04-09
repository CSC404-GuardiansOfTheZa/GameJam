using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathReplay : MonoBehaviour {
    private void OnCollisionEnter(Collision collision) {
        if (!collision.collider.CompareTag("Player")) return;
        
        if (this.transform.CompareTag("EndWall")) {
            Debug.Log("collide with end wall");
            SceneManager.LoadScene(6);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (!this.transform.CompareTag("CoverArea")) {
            Debug.Log("collide with dead object");
            PizzaMan.Instance.Kill();
        }
    }
}
