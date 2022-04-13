using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI playerDeaths;

    void Start() {
        playerScore.SetText(ScoreManager.Instance.Score.ToString());
        this.playerDeaths.SetText(ScoreManager.Instance.Deaths.ToString());
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(LevelManager.Instance.GetLevelScene());
    }

    public void NextLevel() {
        int scene = LevelManager.Instance.GetLevelScene(1);
        if (scene > 0)
            SceneManager.LoadScene(scene);
        else
            this.ReplayGame();
    }

    public void GoToMenu(){
    	SceneManager.LoadScene(0);
    }

    public void QuitGame(){
    	Application.Quit();
    }
}
