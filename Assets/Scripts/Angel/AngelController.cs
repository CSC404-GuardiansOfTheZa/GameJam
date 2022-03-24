using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelController : MonoBehaviour {
    [SerializeField] private float smoothTime = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float detectionRadius = 1f;
    [SerializeField] private ParticleSystem highlightParticles;
    [Header("Leaning")]
    [SerializeField] private float maxLeanSpeed = 50f; // When abs(velocity.x) reaches speedForMaxLean, the angel should then reach the max lean angle (specified below)
    [SerializeField] private float maxLeanAngle = 80f; // Angel can not lean more than maxLeanAngle degrees at full speed

    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    private float lean = 0.0f;
    private int raycastLayerMask;

    private GameObject selectedInteractable = null;

    void Start() {
        this.cam = Camera.main;
        this.raycastLayerMask = LayerMask.GetMask("Interactables");
        highlightParticles.Stop();
    }

    void Update() {
        // // detect interactables in vicinity
        // RaycastHit[] hits = Physics.SphereCastAll(transform.position, this.detectionRadius, Vector3.forward);
        // bool hasInteractable = false;
        // foreach (var hit in hits) {
        //     if (!hit.collider.TryGetComponent<IInteractable>(out IInteractable _))
        //         continue;
        //     hasInteractable = true;
        //     if (selectedInteractable == hit.collider.gameObject) // same gameobject, do nothing
        //         continue;
        //     selectedInteractable?.GetComponent<Outline>()?.HideOutline();
        //     hit.collider.GetComponent<Outline>()?.ShowOutline();
        //     selectedInteractable = hit.collider.gameObject;
        //     print(hit.collider.name);
        // }
        //
        // if (!hasInteractable) {
        //     selectedInteractable?.GetComponent<Outline>()?.HideOutline();
        //     selectedInteractable = null;
        // }
        //
        //
        // // controls for interactable
        // if (Input.GetMouseButtonDown(0))
        //     selectedInteractable?.GetComponent<IInteractable>().Trigger();

        Ray ray = this.cam.ScreenPointToRay(this.cam.WorldToScreenPoint(transform.position));
        bool didRaycastHit = Physics.Raycast(ray, out RaycastHit hit, 100.0f, this.raycastLayerMask);
        if (didRaycastHit) {
            Interactable target = (Interactable) hit.transform.GetComponent(typeof(Interactable));

            bool isTargetActive = false;
            if (target is not null) {
                if (Input.GetMouseButtonDown(0)) {
                    // TODO: somehow disable highlights when target is already active
                    // May need to refactor code and turn IInteractable into an abstract class, not interface
                    target.Trigger();
                }
            }
            
            this.highlightParticles.Play();
            if (selectedInteractable != hit.collider.gameObject) {
                // switching to different gameobject, so adjust the outlines
                selectedInteractable?.GetComponent<Outline>()?.HideOutline();
                hit.collider.GetComponent<Outline>()?.ShowOutline();
                selectedInteractable = hit.collider.gameObject;
            }

        } else {
            this.selectedInteractable?.GetComponent<Outline>()?.HideOutline();
            this.selectedInteractable = null;
            this.highlightParticles.Stop();
        }
        
        Vector3 movementTarget = this.cam.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Mathf.Abs(this.cam.transform.position.z)
            )
        );
        
        transform.position = Vector3.SmoothDamp(transform.position, movementTarget, ref velocity, smoothTime * Time.deltaTime);
        transform.eulerAngles = new Vector3(
            -Mathf.Abs(this.velocity.x) / this.maxLeanSpeed * this.maxLeanAngle,
            Mathf.Lerp(transform.eulerAngles.y, velocity.x < 0 ? 90 : 270, rotateSpeed * Time.deltaTime),
            0);

    }

}
