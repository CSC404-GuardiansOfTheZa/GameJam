using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conductor : MonoBehaviour
{

    [field: SerializeField]
    public int BPM {get; private set;}
    [field: SerializeField]
    public float trackLengthInSeconds {get; private set;}
    [field: SerializeField]
    public float trackOffsetInSeconds {get; private set;}
    [SerializeField]
    private AudioClip musicTrack;

    public float Crotchet { // length of a beat
        get {return 60 / BPM; }
    }
    public float SongPosition { // in seconds
        get {
            if (this.dspTimeStart < 0) {
                return 0.0f;
            }
            return (float)AudioSettings.dspTime - dspTimeStart;
        }
    }

    private float dspTimeStart = -1.0f;
    
    [Space(10)]
    [Header("DEBUG")]
    [SerializeField]
    private bool placeholder = false;

}
