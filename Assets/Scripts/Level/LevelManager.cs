using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    [SerializeField] private bool autoStart=true;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float secondsToWait = 1.0f;
    [SerializeField] private List<int> LevelSceneNumbers; // contains the scene numbers in order (tutorial, level 1, etc.)
    private Scroller scroller;
    
    public delegate void VoidDelegate();
    public event VoidDelegate OnLevelStart;
    public event VoidDelegate OnPause;
    public event VoidDelegate OnResume;
    public event VoidDelegate OnLoadingFinish;
    public event VoidDelegate OnLevelReload;

    public bool Paused { get; private set; }
    public bool Started { get; private set; }
    public int CurrentLevel { get; private set; } // stores the current level index (0 for tutorial, 1 for level 1, etc.)

    public void StartLevel() {
        Started = true;
        this.SetCurrentLevel();
        this.OnLevelStart?.Invoke();
    }

    public void PauseLevel() {
        this.Paused = true;
        this.OnPause?.Invoke();
    }

    public void ResumeLevel() {
        this.Paused = false;
        this.OnResume?.Invoke();
    }

    public void ReloadLevel() {
        Debug.Log("Reloading level");
        this.SetCurrentLevel();
        this.OnLevelReload?.Invoke();
    }

    public void TogglePause() {
        if (this.Paused) this.ResumeLevel();
        else this.PauseLevel();
    }

    public void SaveCheckpointScroll() {
        this.scroller?.SaveCheckpointScroll();
    }

    public int GetLevelScene(int offset=0) {
        int index = CurrentLevel + offset;
        return index >= 0 && index < this.LevelSceneNumbers.Count ?
            this.LevelSceneNumbers[index] :
            -1;
    }
    
    void Awake() { // Set to run before all other scripts
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        StartCoroutine(WaitForLoading());
        this.scroller = this.GetComponent<Scroller>();
    }
    
    void Start(){
        Destroy(GameObject.Find("OptionMusic"));
    }

    void Update() {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        #endif
    }

    private IEnumerator WaitForLoading() {
        if (this.loadingScreen != null) {
            this.loadingScreen.SetActive(true);
            yield return new WaitForSeconds(this.secondsToWait);
            this.loadingScreen.SetActive(false);
        }

        if (this.autoStart) {
            this.StartLevel();
        }
        this.OnLoadingFinish?.Invoke();
    }

    private void SetCurrentLevel() {
        CurrentLevel = this.LevelSceneNumbers.IndexOf(SceneManager.GetActiveScene().buildIndex);
    }
}
