using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{

    // Start is called before the first frame update
    private float musicVolume = 1f;
    public Slider volumeSlider;
    
    void Start(){
    	volumeSlider.value = AudioListener.volume;
    }

    public void updateVolume(Slider slider){
    	musicVolume = slider.value;
    	AudioListener.volume = musicVolume;
    }
}
