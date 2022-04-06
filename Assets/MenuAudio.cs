using UnityEngine;

public class MenuAudio : SingletonMonoBehaviour<MenuAudio> {

    private AudioSource source;

    void Start() {
        source = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect() {
        source.Play();
    }
}
