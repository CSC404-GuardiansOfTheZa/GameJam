using System.Collections;
using System.Collections.Generic;
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

    public delegate void VoidDelegate();
    public event VoidDelegate OnLevelStart;
    public event VoidDelegate OnPause;
    public event VoidDelegate OnResume;
    public event VoidDelegate OnLoadingFinish;

    private bool paused = false;

    public void StartLevel() {
        this.OnLevelStart?.Invoke();
    }

    public void PauseLevel() {
        this.paused = true;
        this.OnPause?.Invoke();
    }

    public void ResumeLevel() {
        this.paused = false;
        this.OnResume?.Invoke();
    }

    public void TogglePause() {
        if (this.paused) this.ResumeLevel();
        else this.PauseLevel();
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
