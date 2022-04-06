using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour {

    void Awake() {
        var objs = Object.FindObjectsOfType<MenuAudio>();

        if (objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {

    }
}
