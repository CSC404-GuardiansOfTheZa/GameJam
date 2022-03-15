using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private List<string> openingScript = new List<string>();
    [Header("Scene Objects")]
    [SerializeField] private DialogueBox dialogueBox;
    [FormerlySerializedAs("clickToContinue")]
    [SerializeField] private FadableText clickToContinueText;
    [SerializeField] private PizzaMan pizza;
    [SerializeField] private Image blackBG;
    [SerializeField] private TriggerEventDispatcher freezeTrigger;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private LimitedDurationInteractable window;
    
    private int scriptIndex = -1;
    private Rigidbody pizzaRb;
    private bool clickToContinue = true;

    // Start is called before the first frame update
    void Start() {
        this.SetDialogueToNextLineInScript();
        this.pizzaRb = this.pizza.GetComponent<Rigidbody>();
        this.pizzaRb.useGravity = false;
        Conductor.Instance.onBeat += this.OnBeat;
        this.freezeTrigger.OnTriggerEnterEvent += this.OnFreeze;
        this.window.OnActivated += this.OnWindowActivated;
    }

    public void OnWindowActivated() {
        LevelManager.Instance.ResumeLevel();
        this.SetDialogueToNextLineInScript();
    }

    // Update is called once per frame
    void Update() {
        if (this.clickToContinue && Input.GetMouseButtonDown(0)) {
            this.SetDialogueToNextLineInScript();
        }
        this.pauseMenu.SetActive(false);
    }

    private void SetDialogueToNextLineInScript() {
        this.scriptIndex++;
        if (this.scriptIndex < this.openingScript.Count) {
            this.StartCoroutine(this.dialogueBox.SetText(this.openingScript[this.scriptIndex]));
        } else {
            this.StartCoroutine(this.dialogueBox.FadeOut());
        }

        StartCoroutine(this.OnScriptAdvance());
    }

    private IEnumerator OnScriptAdvance() {
        switch (this.scriptIndex) {
            // indices start at 0
            case 0:
                break;
            case 1:
                this.StartCoroutine(this.clickToContinueText.FadeOut());
                break;
            case 2:
                yield return new WaitForSeconds(this.dialogueBox.FadeDuration);
                this.pizzaRb.useGravity = true;
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

                this.StartCoroutine(this.clickToContinueText.FadeIn());
                break;
            case 5:
                LevelManager.Instance.StartLevel();
                this.StartCoroutine(this.clickToContinueText.FadeOut());
                this.clickToContinue = false;
                break;
            case 7:
                yield return new WaitForSeconds(this.dialogueBox.FadeDuration);
                this.dialogueBox.fadeDuration = 0.2f; // VERY HACKY BUT IT WORKS BUT IS NOT PROPER OOP
                break;
        }

        yield return null;
    }

    private void OnBeat(int beat) {
        switch (beat) {
            case 2: // before 2nd jump
                SetDialogueToNextLineInScript();
                break;
            case 7: // after 2nd jump, before 3rd jump
                this.SetDialogueToNextLineInScript();
                break;
        }
    }

    private void OnFreeze(Collider other) {
        if (!other.CompareTag("PizzaFeet")) return;

        LevelManager.Instance.PauseLevel();
        this.SetDialogueToNextLineInScript();
    }
}