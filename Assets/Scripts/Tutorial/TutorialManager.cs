using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private List<string> openingScript = new List<string>();
    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private FadableText clickToContinue;
    [SerializeField] private GameObject pizzaGuy;
    private int scriptIndex = -1;

    // Start is called before the first frame update
    void Start() {
        this.SetDialogueToNextLineInScript();
        this.pizzaGuy.GetComponent<Rigidbody>().useGravity = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            this.SetDialogueToNextLineInScript();
        }
    }

    private void SetDialogueToNextLineInScript() {
        this.scriptIndex++;
        if (this.scriptIndex < this.openingScript.Count) {
            this.StartCoroutine(this.dialogueBox.SetText(this.openingScript[this.scriptIndex]));
        } else {
            this.StartCoroutine(this.dialogueBox.FadeAway());
        }

        StartCoroutine(this.OnScriptAdvance());
    }

    private IEnumerator OnScriptAdvance() {
        switch (this.scriptIndex) {
            // indices start at 0
            case 0:
                break;
            case 1:
                this.StartCoroutine(this.clickToContinue.FadeAway());
                break;
            case 2:
                yield return new WaitForSeconds(this.dialogueBox.FadeDuration);
                this.pizzaGuy.GetComponent<Rigidbody>().useGravity = true;
                break;
        }

        yield return null;
    }
}