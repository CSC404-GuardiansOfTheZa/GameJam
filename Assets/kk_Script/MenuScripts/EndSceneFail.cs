using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneFail : MonoBehaviour
{
    // Start is called before the first frame update

    public void ReplayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToMenu(){
    	SceneManager.LoadScene(0);
    }

    public void QuitGame(){
    	Application.Quit();
    }
}
