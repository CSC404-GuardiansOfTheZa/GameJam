using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoardMenu : MonoBehaviour
{
	private int previousScene = 0;

    public void GoBack(){
    	SceneManager.LoadScene(previousScene);
    }
}
