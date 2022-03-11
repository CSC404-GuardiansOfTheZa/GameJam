using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LimitedDurationInteractable : MonoBehaviour, IInteractable {
    [SerializeField] private float activeDuration = 1.0f; // set to <= 0 for infinite duration
    [SerializeField] private bool isActiveAtStart = false;
    [Header("Visual Indicators")]
    [SerializeField] protected Color aboutToDeactivateColor = Color.red;
    [SerializeField] protected List<Renderer> targetRenderers;

    protected Color[] targetRenderersColors;
    protected MaterialPropertyBlock prpblk;

    private bool _isActive;
    public bool IsActive {
        get {
            return this._isActive;
        }
        protected set {
            if (value == this._isActive) return;
            this._isActive = value;
            this.OnActivationChange(false);
        }
    }

    private bool mutex = false;

    protected void Awake() {
        _isActive = this.isActiveAtStart;
        this.OnActivationChange(true);
        this.prpblk = new MaterialPropertyBlock();
        this.targetRenderersColors = new Color[this.targetRenderers.Count];
        for (int i = 0; i < this.targetRenderers.Count; i++) {
            this.targetRenderersColors[i] = this.targetRenderers[i].material.color;
        }
    }

    public void Trigger() {
        StartCoroutine(ActivateForDuration(this.activeDuration));
    }

    protected abstract void OnActivationChange(bool isStart);

    protected IEnumerator ActivateForDuration(float duration) {
        bool isActive_copy = IsActive; // we just use a copy in case mutex doesn't work and there's a race condition
        IsActive = !isActive_copy;
        
        if (!this.mutex && duration > 0) {
            this.mutex = true;
            float elapsedTime = 0.0f;
            while (elapsedTime < duration) 
            {
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
            IsActive = isActive_copy;
            this.mutex = false;
        }
    }

    protected void SetRendererColors(Color targetColor) {
        
    }
}
