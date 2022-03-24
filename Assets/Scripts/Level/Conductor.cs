using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Conductor : MonoBehaviour
{
    private static Conductor _instance;
    public static Conductor Instance { get { return _instance; } } 

    public delegate void onBeatDelegate(int beatNum);
    public event onBeatDelegate onBeat;

    [Header("NOTE: Add music track HERE, not at AudioSource")]
    [SerializeField] private AudioClip musicTrack;
    [field: SerializeField] public float BPM {get; private set;}
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
            } else if (this.isPaused && this.pauseTimeStart > 0.0f) {
                return this.pauseTimeStart - this.dspTimeStart; // this works out if you factor it out trust me bro ~stew
            }
            return AudioSettings.dspTime - dspTimeStart - this.pauseOffset;
        }
    }

    public float SongPositionInBeats => (float) SongPosition / Crotchet;

    private AudioSource asource;
    private double dspTimeStart = -1.0f;
    private int beat = 1;
    private bool isPaused = false;
    private double pauseOffset = 0.0f;
    private double pauseTimeStart = 0.0f;

    public void StartSong() {
       this.asource.Play(); 
       dspTimeStart = AudioSettings.dspTime;
    }

    public void Pause() {
        if (this.isPaused) return;
        
        this.isPaused = true;
        this.asource.Pause();
        this.pauseTimeStart = AudioSettings.dspTime;
    }

    public void Resume() {
        if (!this.isPaused) return;
        
        double resumeTime = AudioSettings.dspTime;
        this.pauseOffset += resumeTime - this.pauseTimeStart;
        this.pauseTimeStart = 0.0f;
        this.asource.UnPause();
        this.isPaused = false;
    }
    
    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        asource = GetComponent<AudioSource>();
        asource.clip = this.musicTrack;
        Crotchet = 60.0f / (float) BPM;
        LevelManager.Instance.OnLevelStart += this.StartSong;
        LevelManager.Instance.OnPause += this.Pause;
        LevelManager.Instance.OnResume += this.Resume;
    }
    
    void Update() {
        if (!this.isPaused && SongPosition > Crotchet * beat && onBeat != null) {
            onBeat(beat);
            beat += 1;
        }
    }

}
