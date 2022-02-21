using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
    	SceneManager.LoadScene(4);
    }

    public void QuitGame(){
    	Application.Quit();
    }

    public void OpenSetting(){
    	SceneManager.LoadScene(2);
    }

    public void OpenScoreBoard(){
    	SceneManager.LoadScene(3);
    }
}
