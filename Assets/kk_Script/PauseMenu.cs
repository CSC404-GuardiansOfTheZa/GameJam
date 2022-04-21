using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    [SerializeField] private bool allowPausing = true;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        LevelManager.Instance.OnPause += this.Pause;
        LevelManager.Instance.OnResume += this.OnResume;
    }

    // Update is called once per frame
    void Update()
    {
     	if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape)){
            if (this.allowPausing)
     		    LevelManager.Instance.TogglePause();
            else
                SceneManager.LoadScene(0);  // go to the main menu
     	}   
    }

    public void Resume() {
	    LevelManager.Instance.ResumeLevel();
    }

    public void OnResume() {
	    if (!this.allowPausing) return;
	    
	    pauseMenuUI.SetActive(false);
    	GameIsPaused = false;
    }

    public void Pause() {
	    if (!this.allowPausing) return;
	    
    	pauseMenuUI.SetActive(true);
    	GameIsPaused = true;
    }

    public void GoToMenu(){
    	SceneManager.LoadScene(0);
    }

    public void QuitGame(){
    	Application.Quit();
    }
}
