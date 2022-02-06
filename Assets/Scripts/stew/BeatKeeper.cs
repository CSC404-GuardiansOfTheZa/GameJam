using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatKeeper : MonoBehaviour
{
    Material mat;
    [SerializeField]
    Color color = Color.yellow;
    [SerializeField]
    [Range(0, 3)]
    private int nthBeat;

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
        if ((beatNum-1)%4 != nthBeat)   return;

        this.color.a = 1;
    }
}
