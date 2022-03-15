using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private List<string> openingScript = new List<string>();
    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private FadableText clickToContinue;
    private int scriptIndex = -1;

    // Start is called before the first frame update
    void Start() {
        this.SetDialogueToNextLineInScript();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            this.SetDialogueToNextLineInScript();
            if (this.scriptIndex == 1) {
                this.StartCoroutine(this.clickToContinue.FadeAway());
            }
        }
    }

    private void SetDialogueToNextLineInScript() {
        this.scriptIndex++;
        if (this.scriptIndex < this.openingScript.Count) {
            this.StartCoroutine(this.dialogueBox.SetText(this.openingScript[this.scriptIndex]));
        } else {
            this.StartCoroutine(this.dialogueBox.FadeAway());
        }
    }
}
