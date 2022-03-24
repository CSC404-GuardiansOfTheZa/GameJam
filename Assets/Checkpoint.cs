using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public float actualXPosition = 0;


    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
            CheckpointManager.Instance.RecordCheckpoint(actualXPosition);
    }
}
