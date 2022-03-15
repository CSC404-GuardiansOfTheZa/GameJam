using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private List<string> openingScript = new List<string>();
    [Header("Scene Objects")]
    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private FadableText clickToContinue;
    [SerializeField] private Rigidbody pizzaGuyRb;
    [SerializeField] private Image blackBG;
    private int scriptIndex = -1;

    // Start is called before the first frame update
    void Start() {
        this.SetDialogueToNextLineInScript();
        this.pizzaGuyRb.useGravity = false;
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
                this.pizzaGuyRb.useGravity = true;
                break;
            case 3:
                break;
            case 4:
                float elapsedTime = 0;
                while (elapsedTime < this.dialogueBox.FadeDuration) {
                    float t = elapsedTime / this.dialogueBox.FadeDuration;
                    this.blackBG.color = Color.Lerp(Color.black, Color.clear, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                break;
        }

        yield return null;
    }
}