using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsMenu : MonoBehaviour
{
    // Start is called before the first frame update
   public void PlayEasyGame(){
    	SceneManager.LoadScene(1);
    }

    public void PlayMediumGame(){
    	SceneManager.LoadScene(1);
    }

   	public void PlayHardGame(){
    	SceneManager.LoadScene(1);
    }

    public void GoBack(){
    	SceneManager.LoadScene(0);
    }
}
