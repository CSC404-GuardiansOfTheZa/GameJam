using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour {
    public void GoBack() {
        MenuAudio.Instance.PlaySoundEffect();
        SceneManager.LoadScene(0);
    }
}
