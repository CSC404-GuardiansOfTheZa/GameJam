using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOnly : MonoBehaviour
{
    void Awake()
    {
        #if !UNITY_EDITOR
        Destroy(this.gameObject);
        #endif
    }
}
