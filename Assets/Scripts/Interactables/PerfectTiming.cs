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

[RequireComponent(typeof(Interactable))]
public class PerfectTiming : MonoBehaviour
{
    // These fields are public so we can have a custom editor for them
    public bool automaticTiming;
    public float beatShouldBeActivatedOn = -2.0f;

    [Header("Tolerances")] 
    [SerializeField] private List<TimingClassData> tolerances; 

    private bool hasBeenTriggered;
    private AudioSource asource;

    private void Start() {
        this.asource = this.GetComponent<AudioSource>();
        this.GetComponent<Interactable>().OnTrigger += this.OnTrigger;
        
        if (this.automaticTiming) {
            float scrollSpeed = LevelManager.Instance.gameObject.GetComponent<Scroller>().scrollSpeed;
            float crotchet = Conductor.Instance.Crotchet;
            float distancePerBeat = scrollSpeed * crotchet;

            this.beatShouldBeActivatedOn = transform.position.x / distancePerBeat; // assumes pizza guy starts at x=0
        }
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
//
// [CustomEditor(typeof(PerfectTiming))]
// public class PerfectTimingEditor : Editor {
//     public override void OnInspectorGUI() {
//         base.OnInspectorGUI();
//         
//         PerfectTiming originalScript = target as PerfectTiming;
//
//         originalScript.automaticTiming = GUILayout.Toggle(originalScript.automaticTiming, "Automatic Timing");
//         if (!originalScript.automaticTiming) {
//             originalScript.beatShouldBeActivatedOn = EditorGUILayout.FloatField(
//                 "Beat Should Be Activated On", 
//                 originalScript.beatShouldBeActivatedOn);
//         }
//     }
// }