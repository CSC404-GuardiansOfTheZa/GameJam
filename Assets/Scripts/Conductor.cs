using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{

    [field: SerializeField]
    public int BPM {get; private set;}
    [field: SerializeField]
    public float trackLengthInSeconds {get; private set;}
    [SerializeField]
    private AudioClip musicTrack;

    public float Crotchet { // length of a beat
        get {return 60 / BPM; }
    }

}
