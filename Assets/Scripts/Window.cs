using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour, IInteractable 
{
    // TODO: merge with ManholeCover.cs
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        // if (Input.GetMouseButtonDown(0)) {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit)) {

        //         Animator anim = hit.transform.GetComponentInParent<Animator>();
        //         if (anim) {
        //             anim.SetTrigger("Next");
        //         }

        //     }
        // }
    }

    public void Trigger() {
        if (this.animator) {
            this.animator.SetTrigger("Next");
        }
        #if UNITY_EDITOR
        else {
            Debug.LogError("Window was triggered, but no Animator was set");
        }
        #endif
    }
}
