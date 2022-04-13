using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneRotation : Interactable {
    private Vector3 startRotation;

    public void Start() {
        this.startRotation = transform.parent.eulerAngles;
        LevelManager.Instance.OnLevelReload += delegate { transform.parent.eulerAngles = this.startRotation; };
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

    protected override void TriggerAction() {
        StartCoroutine(RotateMe(Vector3.up * (-180), 0.5f, transform.parent));
    }
}
