using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInteractable : BinaryInteractable {
    [SerializeField] protected string animationName = "";

    [SerializeField]
    protected float animSpeedMult = 1;

    protected Animator animator;
    protected ParticleSystem particles;


    void Awake() {
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
        animator.speed = animSpeedMult;
    }

    protected override void TriggerAction() {
        IsActive = true;
        
        particles?.Play();
        if (animator) {
            animator.SetTrigger(animationName);
        }
#if UNITY_EDITOR
        else {
            Debug.LogError("Interactable animation was triggered, but no Animator was set");
        }
#endif
    }

    protected override void OnActivationChange(bool isStart) {
        // no use here
    }
}
