using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{

    private Camera _cam;

    [SerializeField] private Animator _myAnimationController;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _myAnimationController = GetComponent<Animator>();

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
                        _myAnimationController.SetTrigger("MoveManHole");
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
