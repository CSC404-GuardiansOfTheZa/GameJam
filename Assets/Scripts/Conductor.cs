using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Conductor : MonoBehaviour
{
    private static Conductor _instance;
    public static Conductor Instance { get { return _instance; } } 


    [Header("NOTE: Add music track HERE, not at AudioSource")]
    [SerializeField] private AudioClip musicTrack;
    [field: SerializeField] public int BPM {get; private set;}
    [field: SerializeField] public float TrackLengthInSeconds {get; private set;}
    [field: SerializeField] public float TrackOffsetInSeconds {get; private set;}
    [field: SerializeField] public int BeatsPerMeasure {get; private set;}

    public float Crotchet { // length of a beat
        get; private set;
    }
    public double SongPosition { // in seconds
        get {
            if (this.dspTimeStart < 0) {
                return 0.0f;
            }
            return AudioSettings.dspTime - dspTimeStart;
        }
    }

    private double dspTimeStart = -1.0f;
    private AudioSource asource;
    
    [Space(10)]
    [Header("DEBUG")]

    private int beat = 1;

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        asource = GetComponent<AudioSource>();
        asource.clip = this.musicTrack;
        Crotchet = 60.0f / (float) BPM;
    }

    double lastBeat = 0; 
    void Start() {
        asource.Play();
        dspTimeStart = AudioSettings.dspTime;
        Debug.Log(Crotchet);
    }

    public delegate void onBeatDelegate(int beatNum);
    public event onBeatDelegate onBeat;
    
    void Update() {
        if (SongPosition > Crotchet * beat && onBeat != null) {
            onBeat(beat);
            beat += 1;
        }
    }

}
