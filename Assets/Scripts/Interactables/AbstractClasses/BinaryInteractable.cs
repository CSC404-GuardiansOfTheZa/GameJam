using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BinaryInteractable : Interactable
{
    [Header("Binary Interactable")]
    [SerializeField] private bool isActiveAtStart = false;
    
    public event LevelManager.VoidDelegate OnActivated;
    public event LevelManager.VoidDelegate OnDeactivated;
    
    protected abstract void OnActivationChange(bool isStart);
    
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

    protected virtual void Awake() {
        _isActive = this.isActiveAtStart;
        this.OnActivationChange(true);
    }
}
