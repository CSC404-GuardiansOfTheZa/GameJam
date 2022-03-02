using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorOfInteractables : MonoBehaviour
{
    [SerializeField] private List<int> layersToActivate;

    private int raycastLayerMask;

    void Start() {
        if (layersToActivate != null) {
            for (int i = 0; i < layersToActivate.Count; i++){
                raycastLayerMask += 1 << layersToActivate[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, raycastLayerMask)) {
                IInteractable target = (IInteractable) hit.transform.GetComponent(typeof(IInteractable));
                if (target != null) {
                    target.Trigger();
                }
            }
        }
    }
}
