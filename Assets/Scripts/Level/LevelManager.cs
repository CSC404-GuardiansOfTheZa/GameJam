using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float secondsToWait = 1.0f;

    public delegate void OnLevelStartDelegate();
    public event OnLevelStartDelegate OnLevelStart;
    
    void Awake() { // Set to run before all other scripts
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        StartCoroutine(WaitForLoading());
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
            yield return new WaitForSecondsRealtime(this.secondsToWait);
            this.loadingScreen.SetActive(false);
        }

        this.OnLevelStart?.Invoke();
    }
}
