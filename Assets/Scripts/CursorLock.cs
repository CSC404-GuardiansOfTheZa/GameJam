using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorLock : MonoBehaviour {
  void Start() {
    LevelManager.Instance.OnPause += this.Unlock;
    LevelManager.Instance.OnResume += this.Lock;
    LevelManager.Instance.OnLoadingFinish += this.Lock;
  }

  private void Update() {
    if (LevelManager.Instance.Started && !LevelManager.Instance.Paused) {
      if (Input.GetKeyDown(KeyCode.Escape)) this.Unlock();
      if (Input.GetMouseButtonDown(0)) this.Lock();
    }
  }

  public void Unlock() {
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
  }

  public void Lock() {
    Cursor.lockState = CursorLockMode.Confined;
    Cursor.visible = false;
  }
}
