using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInteractable : MonoBehaviour, IInteractable {
    [SerializeField] protected string animationName = "";

    protected Animator animator;
    protected ParticleSystem particles;

    void Awake() {
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    public void Trigger() {


        if (animator) {
            animator.SetTrigger(animationName);
        }
#if UNITY_EDITOR
        else {
            Debug.LogError("Interactable animation was triggered, but no Animator was set");
        }

        particles?.Play();


#endif
    }
}
