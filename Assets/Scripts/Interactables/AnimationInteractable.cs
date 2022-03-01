using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInteractable : MonoBehaviour, IInteractable {
    [SerializeField] protected string animationName = "";

    [SerializeField]
    protected float animSpeedMult = 1;

    protected Animator animator;
    protected ParticleSystem particles;

    protected bool isActivated = false;

    void Awake() {
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
        animator.speed = animSpeedMult;
    }

    public void Trigger() {


        if (animator) {
            animator.SetTrigger(animationName);
            isActivated = !isActivated;
        }
#if UNITY_EDITOR
        else {
            Debug.LogError("Interactable animation was triggered, but no Animator was set");
        }

        particles?.Play();


#endif
    }
}
