using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    private AudioSource _audioSource;
    private AudioSource _pauseaudioSource;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        _audioSource = GameObject.Find("LevelManager").GetComponent<AudioSource>();
        _pauseaudioSource = GameObject.Find("PauseMusic").GetComponent<AudioSource>();
        _pauseaudioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
     	if (Input.GetKeyDown(KeyCode.Space)){
     		if (GameIsPaused){
     			Resume();
     		}else{
     			Pause();
     		}
     	}   
    }

    public void Resume(){
    	pauseMenuUI.SetActive(false);
    	Time.timeScale = 1f;
    	GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        PlayMusic();

    }

    void Pause(){
    	pauseMenuUI.SetActive(true);
    	Time.timeScale = 0f;
    	GameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StopMusic();
    }

    public void PlayMusic()
    {
         _audioSource.UnPause();
         _pauseaudioSource.Stop();
    }
 
     public void StopMusic()
    {
         _audioSource.Pause();
         _pauseaudioSource.Play();
    }

    public void GoToMenu(){
    	Time.timeScale = 1f;
    	SceneManager.LoadScene(0);
        _pauseaudioSource.Stop();
    }

    public void QuitGame(){
    	Application.Quit();
    }
}
