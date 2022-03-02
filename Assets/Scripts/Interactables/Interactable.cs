using System;
using System.Collections;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    [SerializeField] private float activeDuration = 1.0f; // set to <= 0 for infinite duration
    [SerializeField] private bool isActiveAtStart = false;

    private bool _isActive;
    public bool IsActive {
        get {
            return this._isActive;
        }
        protected set {
            if (value == this._isActive) return;
            this._isActive = value;
            this.OnActivationChange();
        }
    }

    private bool mutex = false;

    protected void Awake() {
        IsActive = this.isActiveAtStart;
    }

    public virtual void Trigger() {
        if (!this.mutex) {
            this.mutex = true;
            StartCoroutine(ActivateForDuration(this.activeDuration));
        }
    }

    protected abstract void OnActivationChange();

    protected IEnumerator ActivateForDuration(float duration) {
        bool isActive_copy = IsActive; // we just use a copy in case mutex doesn't work and there's a race condition
        IsActive = !isActive_copy;
        if (duration > 0) {
            yield return new WaitForSeconds(duration);
            IsActive = isActive_copy;
        }

        this.mutex = false;
    }
}
