using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour {
  private bool cursorLocked = true;

  void Update() {
    if (Input.GetKeyDown(KeyCode.Escape))
      this.cursorLocked = !this.cursorLocked;
    
    Cursor.lockState = this.cursorLocked ? CursorLockMode.Confined : CursorLockMode.None;
    Cursor.visible = !this.cursorLocked;
  }
}
