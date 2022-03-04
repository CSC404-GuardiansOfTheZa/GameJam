using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrant : LimitedDurationInteractable {
    [SerializeField] private WaterSpout spout;
    private AudioSource _audioSource;
    public AudioClip clip;

    public void Start() {
        if (this.spout == null) {
            Debug.LogError("Error! spout is not set in Hydrant object!");
            Destroy(this);
        }
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void OnActivationChange(bool isStart) {
        this.spout.ToggleSpout(this.IsActive);
        if (!isStart)
            _audioSource.PlayOneShot(clip);
    }
}
