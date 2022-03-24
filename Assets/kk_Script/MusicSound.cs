using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSound : MonoBehaviour
{
    private AudioSource _audioSource;
    private GameObject[] other;

    private void Awake()
    {
         other = GameObject.FindGameObjectsWithTag("OptionMusic");

         foreach (GameObject oneOther in other) {
             if (oneOther.scene.buildIndex == -1) { 
                 Destroy(gameObject);
             }

             DontDestroyOnLoad(transform.gameObject);
             _audioSource = GameObject.Find("OptionMusic")?.GetComponent<AudioSource>();
         }
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource?.Play();
    }
 
    public void StopMusic()
    {
        _audioSource?.Stop();
    }

}
