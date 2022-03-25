using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    private static CheckpointManager _instance;
    public static CheckpointManager Instance { get { return _instance; } }

    [SerializeField] private Animator checkpointReachedIndicator;
    [SerializeField] private float secondsBeforeRespawn = 3.0f;

    private void Awake() {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        PizzaMan.Instance.OnKilled += this.OnRespawn;
    }

    public void RecordCheckpoint() {
        PizzaMan.Instance.SetNextSpawnToCurrentPos();
        LevelManager.Instance.SaveCheckpointScroll();
        checkpointReachedIndicator.SetTrigger("Show");
        // todo: also record position of the song
    }

    private void OnRespawn() {
        StartCoroutine(this.Respawn());
    }

    public IEnumerator Respawn() {
        PizzaMan.Instance.OnKilled -= this.OnRespawn;
        yield return new WaitForSeconds(this.secondsBeforeRespawn);
        PizzaMan.Instance.OnKilled += this.OnRespawn;
        LevelManager.Instance.ReloadLevel();
    }
}
