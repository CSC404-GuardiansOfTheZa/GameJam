using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour {
    [SerializeField] private Vector3 rotationSpeedPerAxis; // degrees/second

    private void Update() {
        transform.eulerAngles += this.rotationSpeedPerAxis * Time.deltaTime;
    }
}
