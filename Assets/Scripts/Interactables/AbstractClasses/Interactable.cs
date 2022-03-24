using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    [SerializeField] private float beatShouldBeActivatedOn = -1.0f;

    public event LevelManager.VoidDelegate OnTrigger;

    public void Trigger() {
        Debug.Log("Triggered!");
        
        // show message for the beat
        if (this.beatShouldBeActivatedOn > 0) {
            float triggeredBeat = Conductor.Instance.SongPositionInBeats;
            float diff = this.beatShouldBeActivatedOn - triggeredBeat; // positive ==> early, negative ==> late
            // TODO: have messages actually appear in-game.
            // TODO: 
            if (Mathf.Approximately(diff, 0)) {
                Debug.Log("Perfect!");
            }
        }
        
        // we use another method instead of making Trigger() virtual
        // because we don't want subclasses to ever replace Trigger()
        // it also allows functions to "self-trigger" without the "perfect timing!" coming into play
        this.TriggerAction();
        this.OnTrigger?.Invoke();
    }

    protected abstract void TriggerAction();
}
