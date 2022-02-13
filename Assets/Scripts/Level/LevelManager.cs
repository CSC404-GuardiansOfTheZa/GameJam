using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }
    private Scroller levelScroller;
    
    void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        
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
