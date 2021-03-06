using System;
using System.Collections;
using UnityEngine;

public class Window : LimitedDurationInteractable {
    [OnChangedCall("SetStartWindow")]
    [Header("Window Settings")]
    [SerializeField] private float secondsToOpen = 2.0f;
    [SerializeField] private Collider platformCol;
    [Header("Rotation")]
    [SerializeField] private Transform pivot;
    [SerializeField] private float closedXRotation = 0.0f;
    [SerializeField] private float openXRotation = 90.0f;
    [Header("Scale")]
    [SerializeField] private float scaleMultiplierWhenOpen = 2.5f;
    [Header("Start the window open")]
    [SerializeField] private bool startOpen;
    private AudioSource _audioSource;
    public AudioClip clip;


    public void Start() {
        _audioSource = GetComponent<AudioSource>();
        this.StartCoroutine(this.SetWindow());
    }

    protected override void OnActivationChange(bool isStart) {
        this.platformCol.enabled = this.IsActive;
        StartCoroutine(this.SetWindow());
        if (!isStart)
            _audioSource.PlayOneShot(clip);
    }

    public void SetStartWindow() { // is used in the OnChangedCall statement
        float xRotation = !this.startOpen ? this.openXRotation : this.closedXRotation;
        this.pivot.eulerAngles = new Vector3(xRotation, 0, 0);
    }

    IEnumerator SetWindow() {
        float startRot = transform.eulerAngles.x;
        float endRot = this.IsActive ? this.openXRotation : this.closedXRotation;

        float startScale = transform.localScale.y;
        float endScale = this.IsActive ? this.scaleMultiplierWhenOpen : 1;

        float timeElapsed = 0.0f;
        while (timeElapsed < this.secondsToOpen) {
            if (LevelManager.Instance.Paused) {
                yield return null;
                continue;
            }
            
            float t = timeElapsed / this.secondsToOpen;
            float xRotation = this.EasedLerp(startRot, endRot, t);
            this.pivot.eulerAngles = new Vector3(xRotation, 0, 0);

            Vector3 pivotScale = this.pivot.localScale;
            pivotScale.y = this.EasedLerp(startScale, endScale, t);
            pivot.localScale = pivotScale;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        this.pivot.eulerAngles = new Vector3(endRot, 0, 0);
    }

    private float EasedLerp(float a, float b, float t) {
        // https://easings.net/en#easeOutBack
        float newT = 1 - Mathf.Pow(1 - t, 3);
        return Mathf.Lerp(a, b, newT);
    }
}
