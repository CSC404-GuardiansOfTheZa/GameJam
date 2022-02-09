using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManholeCover : MonoBehaviour, IInteractable
{
    //TODO: merge with Window.cs

    // private Camera _cam;
    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Trigger() {
        if (animator){
            animator.SetTrigger("MoveManHole");
        }
        #if UNITY_EDITOR
        else {
            Debug.LogError("Window was triggered, but no Animator was set");
        }
        #endif
    }

    // void HitAtMousePos()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         RaycastHit hit;
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         if (Physics.Raycast(ray, out hit, 100f))
    //         {
    //             if (hit.transform != null)
    //             {
    //                 if (hit.collider.CompareTag("Drag_Cover"))
    //                 {
    //                     Animator anim = hit.transform.GetComponentInParent<Animator>();
                        
    //                 }
    //                 else
    //                 {
    //                     return;
    //                 }
    //             }
    //         }
    //     }
    // }
}