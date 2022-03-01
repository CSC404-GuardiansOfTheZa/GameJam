using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour {
    [SerializeField]
    private Transform dynamicLevelParent;
    [Header("Scroll Settings")]
    [SerializeField]
    public float scrollSpeed = 10; // Measured in units/second

    private bool isInitialized = false;
    private float progress = 0; // 0 to 1
    private float trackLength; // Value taken from Conductor

    public void Init() {
        this.isInitialized = true;
        this.progress = 0;
        this.trackLength = Conductor.Instance.TrackLengthInSeconds;
    }

    public void Stop() {
        isInitialized = false;
    }

    void Update() {
        float deltaTime = Time.deltaTime;
        if (isInitialized && progress < 1.0f) {
            progress += deltaTime / this.trackLength;
            this.dynamicLevelParent.localPosition += Vector3.left * scrollSpeed * deltaTime;
        }
    }
}
