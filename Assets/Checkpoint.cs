using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public float actualXPosition = 0;

    private bool hasGoneThru = false;


    void OnTriggerEnter(Collider other) {
        if (hasGoneThru)
            return;
        if (other.tag != "Player")
            return;
        hasGoneThru = true;
        CheckpointManager.Instance.RecordCheckpoint(transform.position.x);
    }
}
