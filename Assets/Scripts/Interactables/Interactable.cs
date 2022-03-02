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
            
            if (this._isActive) {
                this.OnActivate();
            } else {
                this.OnDeactivate();
            }
        }
    }

    protected void Awake() {
        IsActive = this.isActiveAtStart;
    }

    public virtual void Trigger() {
        StartCoroutine(ActivateForDuration(this.activeDuration));
    }

    protected abstract void OnActivate();
    protected abstract void OnDeactivate();

    protected IEnumerator ActivateForDuration(float duration) {
        bool isActive_copy = IsActive;
        IsActive = !isActive_copy;
        if (duration > 0) {
            yield return new WaitForSeconds(duration);
            IsActive = isActive_copy;
        } else {
            yield return null;
        }
    }
}
