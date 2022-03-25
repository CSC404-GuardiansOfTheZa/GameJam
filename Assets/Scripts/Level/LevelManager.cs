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
    [SerializeField] private Scroller scroller;

    public delegate void VoidDelegate();
    public event VoidDelegate OnLevelStart;
    public event VoidDelegate OnPause;
    public event VoidDelegate OnResume;
    public event VoidDelegate OnLoadingFinish;
    public event VoidDelegate OnLevelReload;

    public bool Paused { get; private set; }
    public bool Started { get; private set; }

    public void StartLevel() {
        Started = true;
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
        this.OnLevelReload?.Invoke();
    }

    public void TogglePause() {
        if (this.Paused) this.ResumeLevel();
        else this.PauseLevel();
    }

    public void SaveCheckpointScroll() {
        this.scroller.SaveCheckpointScroll();
    }
    
    void Awake() { // Set to run before all other scripts
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        StartCoroutine(WaitForLoading());
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
}
