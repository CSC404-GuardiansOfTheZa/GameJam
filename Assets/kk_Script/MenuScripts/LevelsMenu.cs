using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    MusicSound music;
    void Start(){
        music = GameObject.Find("OptionMusic").GetComponent<MusicSound>();
    }

    public void PlayScene(int scene){
        this.music.StopMusic();
        SceneManager.LoadScene(scene);
    }

    public void GoBack(){
    	SceneManager.LoadScene(0);
    }

    public void StopMusic(){
        music.StopMusic();
    }
}
