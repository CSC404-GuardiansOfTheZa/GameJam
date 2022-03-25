using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    private static CheckpointManager _instance;
    public static CheckpointManager Instance { get { return _instance; } }

    [SerializeField] private Transform levelParent;
    [SerializeField] private Animator checkpointReachedIndicator;
    [SerializeField] private float secondsBeforeRespawn = 3.0f;

    private float checkpointXPos;
    
    private void Awake() {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        PizzaMan.Instance.OnKilled += delegate { StartCoroutine(this.Respawn()); };
    }

    public void RecordCheckpoint(float xPos) {
        checkpointXPos = xPos;
        Debug.Log("checkpoint at " + xPos);
        checkpointReachedIndicator.SetTrigger("Show");
    }

    public IEnumerator Respawn() {
        yield return new WaitForSeconds(this.secondsBeforeRespawn);
        
        // todo: have checkpoints store both an X *and* Y pos, and reload the level there.
        levelParent.localPosition = Vector3.left * checkpointXPos;
        print("RESPAWN AT CHECKPOINT" + checkpointXPos);
        
        LevelManager.Instance.ResumeLevel();
    }
}
