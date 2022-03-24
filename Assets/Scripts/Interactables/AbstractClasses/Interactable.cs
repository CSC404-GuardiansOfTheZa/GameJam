using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    public event LevelManager.VoidDelegate OnTrigger;

    public void Trigger() {
        Debug.Log("Triggered!");
        
        // we use another method instead of making Trigger() virtual
        // because we don't want subclasses to ever replace Trigger()
        // it also allows functions to "self-trigger" without the "perfect timing!" etc. happening
        this.TriggerAction();
        this.OnTrigger?.Invoke();
    }

    protected abstract void TriggerAction();
}
