using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneRotation : MonoBehaviour, IInteractable {
	[SerializeField] private float rotateTime = 0.5f;
	private bool rotate = false;

    public void Trigger() {
	    if (!rotate) {
		    this.rotate = true;
    		StartCoroutine(RotateMe(Vector3.up * (-180), this.rotateTime));
	    }
    }

    IEnumerator RotateMe(Vector3 byAngles, float inTime)
     {
         var fromAngle = transform.rotation;
         var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
         for (var t = 0f; t <= 1; t += Time.deltaTime / inTime)
         {
             transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
           
             yield return null;
         }
 
         transform.rotation = toAngle;
         rotate = false;
     }

}
