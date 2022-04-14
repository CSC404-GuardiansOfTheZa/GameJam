using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    void Start(){
        PlayerPrefs.SetString("prevScene", "Begin");
        PlayerPrefs.Save();
    }

    public void PlayGame() {
        MenuAudio.Instance.PlaySoundEffect();
        SceneManager.LoadScene(4);
    }

    public void QuitGame() {
        MenuAudio.Instance.PlaySoundEffect();
        Application.Quit();
    }

    public void OpenSetting() {
        MenuAudio.Instance.PlaySoundEffect();
        SceneManager.LoadScene(2);

    }

    public void OpenScoreBoard() {
        MenuAudio.Instance.PlaySoundEffect();
        SceneManager.LoadScene(3);
    }
}
