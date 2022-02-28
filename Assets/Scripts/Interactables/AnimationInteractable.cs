using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] protected string animationName = "";

    protected Animator animator;    

    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Trigger() {
        if (animator) {
            animator.SetTrigger(animationName);
        }
        #if UNITY_EDITOR
        else {
            Debug.LogError("Interactable animation was triggered, but no Animator was set");
        }
        #endif
    }
}
