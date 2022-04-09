using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerScoreObj;

    void Start() {
        playerScoreObj.GetComponent<TMPro.TextMeshProUGUI>().text = "Your Score: " + ScoreManager.Instance.Score;
    }

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
