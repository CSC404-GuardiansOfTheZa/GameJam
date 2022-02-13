using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Goes onto a "BeatKeeper" game object.
* When the Beat function is called, the BeatKeeper flashes"
**/
public class BeatKeeper : MonoBehaviour
{
    Material mat;
    [SerializeField] Color color = Color.yellow;
    [SerializeField] [Range(1, 8)] private int nthBeat; // The nth beat in a measure. Counts from 1 up.

    void Start() {
        this.mat = GetComponent<Renderer>().material;
        this.color.a = 0;
        mat.color = this.color;
        Conductor.Instance.onBeat += this.Beat;
    }

    void Update() {
        if (this.color.a > 0) {
            this.color.a -= 3.0f * Time.deltaTime;
            mat.color = this.color;
        }
    }

    public void Beat(int beatNum) {
        if ((beatNum-1)%Conductor.Instance.BeatsPerMeasure != nthBeat-1)
            return;

        this.color.a = 1;
    }
}
