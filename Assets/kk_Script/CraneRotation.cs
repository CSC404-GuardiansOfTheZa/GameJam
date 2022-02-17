using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneRotation : MonoBehaviour
{
	private Transform _trans;
	private float lerpTime = 1;
	private bool rotate = false;
    // Start is called before the first frame update
    void Start()
    {
        _trans = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetMouseButtonDown(0) && !rotate){
    		rotate = true;
    		StartCoroutine(RotateMe(Vector3.up * (-180), 5f));
    	}        
    }

    IEnumerator RotateMe(Vector3 byAngles, float inTime)
     {
         var fromAngle = _trans.rotation;
         var toAngle = Quaternion.Euler(_trans.eulerAngles + byAngles);
         for (var t = 0f; t <= 1; t += Time.deltaTime / inTime)
         {
             _trans.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
           
             yield return null;
         }
 
         _trans.rotation = toAngle;
         rotate = false;
     }

}
