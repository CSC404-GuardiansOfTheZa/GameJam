using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private bool hasGoneThru = false;

    void OnTriggerEnter(Collider other) {
        if (hasGoneThru)
            return;
        if (other.tag != "Player")
            return;
        hasGoneThru = true;
        CheckpointManager.Instance.RecordCheckpoint();
    }
}
