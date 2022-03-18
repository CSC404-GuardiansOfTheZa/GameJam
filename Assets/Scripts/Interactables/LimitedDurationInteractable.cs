using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LimitedDurationInteractable : MonoBehaviour, IInteractable {
    [SerializeField] private float activeDuration = 1.0f; // set to <= 0 for infinite duration
    [SerializeField] private bool isActiveAtStart = false;
    [Header("Visual Indicators")]
    [SerializeField] protected Color aboutToDeactivateColor = Color.red;
    [SerializeField] protected List<Renderer> targetRenderers;

    public event LevelManager.VoidDelegate OnActivated;
    public event LevelManager.VoidDelegate OnDeactivated;

    protected Color[] targetRenderersColors;
    protected MaterialPropertyBlock prpblk;
    protected bool isPaused { get; private set; }

    private bool _isActive;
    public bool IsActive {
        get {
            return this._isActive;
        }
        protected set {
            if (value == this._isActive) return;
            this._isActive = value;
            if (this._isActive) OnActivated?.Invoke();
            else OnDeactivated?.Invoke();
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
        if (LevelManager.Instance) {
            LevelManager.Instance.OnPause += delegate { isPaused = true; };
            LevelManager.Instance.OnResume += delegate { isPaused = false; };
        }

    }

    public void Trigger() {
        StartCoroutine(ActivateForDuration(this.activeDuration));
    }

    protected abstract void OnActivationChange(bool isStart);

    protected IEnumerator ActivateForDuration(float duration) {
        bool isActive_copy = IsActive; // we just use a copy in case mutex doesn't work and there's a race condition

        if (duration > 0) {
            if (this.mutex) {
                yield break;
            }

            this.mutex = true;
            IsActive = !isActive_copy;
            float elapsedTime = 0.0f;
            while (elapsedTime < duration) {
                if (isPaused) {
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
            IsActive = isActive_copy;
            this.mutex = false;
        } else {
            IsActive = !isActive_copy;
        }
    }

    protected void SetRendererColors(Color targetColor) {

    }
}
