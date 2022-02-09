using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private Scroller levelScroller;
    
    void Awake()
    {
        levelScroller = GetComponent<Scroller>();
        levelScroller.Init();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Lose() {
        Debug.Log("LOSE LOSE LOSE LOSE!");
        levelScroller.Stop();
    }
}
