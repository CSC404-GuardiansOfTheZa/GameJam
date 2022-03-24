using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LimitedDurationInteractable : BinaryInteractable {
    [Header("Limited Duration Interactable")]
    [SerializeField] private float activeDuration = 1.0f; // set to <= 0 for infinite duration
    [SerializeField] protected Color aboutToDeactivateColor = Color.red;
    [SerializeField] protected List<Renderer> targetRenderers;

    

    protected Color[] targetRenderersColors;
    protected MaterialPropertyBlock prpblk;
    private bool mutex = false;

    protected override void Awake() {
        base.Awake();
        
        this.prpblk = new MaterialPropertyBlock();
        this.targetRenderersColors = new Color[this.targetRenderers.Count];
        for (int i = 0; i < this.targetRenderers.Count; i++) {
            this.targetRenderersColors[i] = this.targetRenderers[i].material.color;
        }
    }

    protected override void TriggerAction() {
        StartCoroutine(ActivateForDuration(this.activeDuration));
    }

    protected IEnumerator ActivateForDuration(float duration) {
        bool isActiveCopy = IsActive; // we just use a copy in case mutex doesn't work and there's a race condition

        if (duration > 0) {
            if (this.mutex) {
                yield break;
            }

            this.mutex = true;
            IsActive = !isActiveCopy;
            float elapsedTime = 0.0f;
            while (elapsedTime < duration) {
                if (LevelManager.Instance.Paused) {
                    yield return null;
                    continue;
                }
                // Turn progressively red
                for (int i = 0; i < this.targetRenderers.Count; i++) {
                    var rnd = this.targetRenderers[i];
                    var startColor = this.targetRenderersColors[i];
                    rnd.GetPropertyBlock(this.prpblk);
                    rnd.material.color = Color.Lerp(startColor, this.aboutToDeactivateColor, elapsedTime / duration);
                    rnd.GetPropertyBlock(this.prpblk);
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            for (int i = 0; i < this.targetRenderers.Count; i++) {
                var rnd = this.targetRenderers[i];
                var startColor = this.targetRenderersColors[i];
                rnd.GetPropertyBlock(this.prpblk);
                rnd.material.color = startColor;
                rnd.GetPropertyBlock(this.prpblk);
            }
            IsActive = isActiveCopy;
            this.mutex = false;
        } else {
            IsActive = !isActiveCopy;
        }
    }
}
