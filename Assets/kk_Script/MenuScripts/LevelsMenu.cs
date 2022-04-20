using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsMenu : MonoBehaviour {
    // Start is called before the first frame update
    MusicSound music;
    void Start() {
        music = GameObject.Find("OptionMusic").GetComponent<MusicSound>();
    }

    public void PlayScene(int scene) {
        MenuAudio.Instance.PlaySoundEffect();
        this.music?.StopMusic();
        SceneManager.LoadScene(scene);
        PlayerPrefs.SetInt("playScene", scene);
        PlayerPrefs.Save();
    }

    public void GoBack() {
        MenuAudio.Instance.PlaySoundEffect();
        SceneManager.LoadScene(0);
    }

    public void StopMusic() {
        music.StopMusic();
    }
}
