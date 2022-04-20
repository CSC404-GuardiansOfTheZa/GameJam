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
     	if (this.allowPausing && Input.GetKeyDown(KeyCode.Space)){
     		LevelManager.Instance.TogglePause();
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
