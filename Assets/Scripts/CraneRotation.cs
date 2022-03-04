using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneRotation : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "Crane") {
                    Transform rotateObj = hit.transform.parent.transform.Find("Stem");
                    if(rotateObj.rotation.y == 1 || rotateObj.rotation.y == 0 || rotateObj.rotation.y == -1){
                        StartCoroutine(RotateMe(Vector3.up * (-180), 0.5f, rotateObj));
                    }
                }

            }
        }        
    }

    public void Trigger() {
        // Transform rotateObj = transform.parent.transform.Find("Stem");
        Debug.Log("hit!");
        Transform rotateObj = transform.parent;
        // if(rotateObj.rotation.y == 1 || rotateObj.rotation.y == 0 || rotateObj.rotation.y == -1){
            StartCoroutine(RotateMe(Vector3.up * (-180), 0.5f, rotateObj));
        // }
    }

    IEnumerator RotateMe(Vector3 byAngles, float inTime, Transform tnsfm)
     {
         var fromAngle = tnsfm.rotation;
         var toAngle = Quaternion.Euler(tnsfm.eulerAngles + byAngles);
         for (var t = 0f; t <= 1; t += Time.deltaTime / inTime)
         {
             tnsfm.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
           
             yield return null;
         }
            
         tnsfm.rotation = toAngle;
     }

}
