using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerEventDispatcher : MonoBehaviour {
    private Collider col;

    private void Awake() {
        this.col = this.GetComponent<Collider>();
    }

    public delegate void TriggerDelegate(Collider other);
    public event TriggerDelegate OnTriggerEnterEvent;
    private void OnTriggerEnter(Collider other) {
        this.OnTriggerEnterEvent?.Invoke(other);
    }
    
    public event TriggerDelegate OnTriggerExitEvent;
    private void OnTriggerExit(Collider other) {
        this.OnTriggerExitEvent?.Invoke(other);
    }
}