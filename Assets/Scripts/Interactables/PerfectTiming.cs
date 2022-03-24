using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TimingClassData {
    public string name;
    public Sprite sprite;
    public float toleranceInBeats;
    public AudioClip audio;
}

public class PerfectTiming : MonoBehaviour
{
    // These fields are public so we can have a custom editor for them
    [SerializeField] private bool areTimingsEnabled = true;
    [SerializeField] private Interactable parentInteractable;

    [Header("Tolerances")] 
    [SerializeField] private List<TimingClassData> tolerances; 

    private bool hasBeenTriggered;
    private AudioSource asource;
    private float beatShouldBeActivatedOn = -2.0f;

    private void Start() {
        this.asource = this.GetComponent<AudioSource>();
        
        float scrollSpeed = LevelManager.Instance.gameObject.GetComponent<Scroller>().scrollSpeed;
        float crotchet = Conductor.Instance.Crotchet;
        float distancePerBeat = scrollSpeed * crotchet;
        this.beatShouldBeActivatedOn = transform.position.x / distancePerBeat; // assumes pizza guy starts at x=0
        
        parentInteractable.OnTrigger += this.OnTrigger;
    }

    private void OnTrigger() {
        if (this.hasBeenTriggered || this.beatShouldBeActivatedOn <= 0) return;
        this.hasBeenTriggered = true;
        
        Debug.LogFormat("CheckingTimings: {0}", this.beatShouldBeActivatedOn);
        float triggeredBeat = Conductor.Instance.SongPositionInBeats;
        float diff = this.beatShouldBeActivatedOn - triggeredBeat; // positive ==> early, negative ==> late

        foreach (var tolerance in this.tolerances) {
            if (diff < tolerance.toleranceInBeats) {
                Debug.Log(tolerance.name);
                if (tolerance.audio is not null)
                    this.asource.PlayOneShot(tolerance.audio, 5.0f);
                else 
                    Debug.Log("AAAAAAAAAAAAAAAAAAAA");
                
                // todo: actually show something
                break;
            }
        }
        
    }
}
