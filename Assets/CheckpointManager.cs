using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    private static CheckpointManager _instance;
    public static CheckpointManager Instance { get { return _instance; } }

    [SerializeField]
    private Transform levelParent;
    private float checkpointXPos;

    [SerializeField] private Animator checkpointReachedIndicator;

    private void Awake() {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

    }

    public void RecordCheckpoint(float xPos) {
        checkpointXPos = xPos;
        Debug.Log("checkpoint at " + xPos);
        checkpointReachedIndicator.SetTrigger("Show");
    }

    public void Respawn() {
        levelParent.localPosition = Vector3.left * checkpointXPos;
        print("RESPAWN AT CHECKPOINT" + checkpointXPos);
        GetComponent<Conductor>().StartSong();
    }
}
