using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInteractable : Interactable
{
    [SerializeField] protected string animationName = "";

    protected Animator animator;    

    void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void Trigger() {
        base.Trigger();
        // TODO: fix
        if (animator) {
            animator.SetTrigger(animationName);
        }
        #if UNITY_EDITOR
        else {
            Debug.LogError("Interactable animation was triggered, but no Animator was set");
        }
        #endif
    }

    protected override void OnActivate() {
        throw new System.NotImplementedException();
    }

    protected override void OnDeactivate() {
        throw new System.NotImplementedException();
    }
}
