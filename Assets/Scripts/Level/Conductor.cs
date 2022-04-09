using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Conductor : MonoBehaviour {
    private static Conductor _instance;
    public static Conductor Instance { get { return _instance; } }

    public delegate void onBeatDelegate(int beatNum);
    public event onBeatDelegate onBeat;
    public event onBeatDelegate onOffBeat;

    [Header("NOTE: Add music track HERE, not at AudioSource")]
    [SerializeField] private AudioClip musicTrack;
    [field: SerializeField] public float BPM { get; private set; }
    [field: SerializeField] public float TrackLengthInSeconds { get; private set; }
    [field: SerializeField] public int BeatsPerMeasure { get; private set; }

    public float Crotchet { // length of a beat
        get; private set;
    }
    public double SongPosition { // in seconds
        get {
            if (this.dspTimeStart < 0) {
                return 0.0f;
            } else if (this.isPaused && this.pauseTimeStart > 0.0f) {
                return this.dspCheckpointSongPosition + (this.pauseTimeStart - this.dspTimeStart); // this works out if you factor it out trust me bro ~stew
            }
            return this.dspCheckpointSongPosition + (AudioSettings.dspTime - this.dspTimeStart - this.pauseOffset);
        }
    }

    public float SongPositionInBeats => (float) SongPosition / Crotchet;

    private AudioSource asource;
    private double dspTimeStart = -1.0f;
    private int beat = 1;
    private bool isPaused = false;
    private double pauseOffset = 0.0f;
    private double pauseTimeStart = 0.0f;
    private float asourceTime;
    private double dspCheckpointSongPosition;
    private int checkpointBeat;
    private double checkpointPauseOffset;
    private bool wasOffBeatTriggered;

    public void StartSong() {
        this.asource.Play();
        dspTimeStart = AudioSettings.dspTime;
        beat = 1;
        this.SavePlaybackTime();
        #if UNITY_EDITOR
        DebugPanel.Instance?.AddDebugLog("beat", () => this.SongPositionInBeats.ToString());
        DebugPanel.Instance?.AddDebugLog("beat (real)", () => this.beat.ToString());
        #endif
    }

    public void Pause(bool addToOffset) {
        if (this.isPaused) return;

        this.isPaused = true;
        this.asource.Pause();
        if (addToOffset) {
            this.pauseTimeStart = AudioSettings.dspTime;
        }
    }

    public void Resume(bool addToOffset) {
        if (!this.isPaused) return;

        if (addToOffset) {
            double resumeTime = AudioSettings.dspTime;
            this.pauseOffset += resumeTime - this.pauseTimeStart;
            this.pauseTimeStart = 0.0f;
        }
        
        this.asource.UnPause();
        this.isPaused = false;
    }

    public void SavePlaybackTime() {
        this.asourceTime = this.asource.time;
        this.dspCheckpointSongPosition = SongPosition;
        this.dspTimeStart = AudioSettings.dspTime;
        this.checkpointBeat = this.beat;
        this.checkpointPauseOffset = this.pauseOffset;
    }

    public void Reload() {
        this.asource.time = this.asourceTime;
        this.beat = this.checkpointBeat;
        this.dspTimeStart = AudioSettings.dspTime;
        this.pauseOffset = this.checkpointPauseOffset;
        this.Resume(false);
    }

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        asource = GetComponent<AudioSource>();
        asource.clip = this.musicTrack;
        Crotchet = 60.0f / (float)BPM;
        LevelManager.Instance.OnLevelStart += this.StartSong;
        LevelManager.Instance.OnPause += delegate { this.Pause(true); };
        LevelManager.Instance.OnResume += delegate { this.Resume(true); };
        PizzaMan.Instance.OnKilled += delegate { this.Pause(false); };
        LevelManager.Instance.OnLevelReload += this.Reload;
    }

    void Update() {
        if (this.isPaused) return;
        if (this.SongPosition > this.Crotchet * this.beat) {
            this.onBeat?.Invoke(this.beat);
            this.beat += 1;
            this.wasOffBeatTriggered = false;
        } else if (!this.wasOffBeatTriggered && this.SongPosition > this.Crotchet * (this.beat - 0.5f)) {
            this.onOffBeat?.Invoke(this.beat);
            this.wasOffBeatTriggered = true;
        }
    }

}
