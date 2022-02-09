using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{

    private Camera _cam;

    private Animator _myAnimationController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        HitAtMousePos();
    }

    void HitAtMousePos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform != null)
                {
                    if (hit.collider.CompareTag("Drag_Cover"))
                    {
                        Animator anim = hit.transform.GetComponentInParent<Animator>();
                        if (anim){
                            anim.SetTrigger("MoveManHole");
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
}