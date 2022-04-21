using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private List<string> openingScript = new List<string>();
    [SerializeField] private List<int> beatsToContinueTutorialOn = new List<int>();
    [Header("Scene Objects")]
    [SerializeField] private DialogueBox dialogueBox;
    [FormerlySerializedAs("clickToContinue")]
    [SerializeField] private FadableText clickToContinueText;
    [SerializeField] private PizzaMan pizza;
    [SerializeField] private Image blackBG;
    [SerializeField] private TriggerEventDispatcher freezeTrigger;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private LimitedDurationInteractable window;
    [SerializeField] private Camera subcam;

    [Header(
        "THE BEYOND REALM"
    )]
    [SerializeField] private TriggerEventDispatcher window2Freeze;
    [SerializeField] private LimitedDurationInteractable window2;
    [SerializeField] private TriggerEventDispatcher window3Freeze;
    [SerializeField] private LimitedDurationInteractable window3;
    [SerializeField] private TriggerEventDispatcher umbrellaFreeze;
    [SerializeField] private Umbrella umbrella;

    private int scriptIndex = -1;
    private Rigidbody pizzaRb;
    private bool clickToContinue = false;
    private bool allowToClick = true;

    // Start is called before the first frame update
    void Start() {
        this.pizzaRb = this.pizza.GetComponent<Rigidbody>();
        this.pizzaRb.useGravity = false;
        Conductor.Instance.onBeat += this.OnBeat;
        this.StartCoroutine(this.clickToContinueText.FadeOut());
        LevelManager.Instance.OnLoadingFinish += this.SetDialogueToNextLineInScript;

        this.freezeTrigger.OnTriggerEnterEvent += delegate(Collider c) { this.OnFreeze(c, this.window.IsActive); }; 
        this.window.OnActivated += this.OnWindowActivated;
        this.window.SetClickable(false);

        this.window2Freeze.OnTriggerEnterEvent += this.OnFreeze;
        this.window2.OnActivated += this.OnWindowActivated;
        this.window2.SetClickable(false);

        this.window3Freeze.OnTriggerEnterEvent += this.OnFreeze;
        this.window3.OnActivated += this.OnWindowActivated;
        this.window3.SetClickable(false);

        this.umbrellaFreeze.OnTriggerEnterEvent += this.OnFreeze;
        this.umbrella.OnActivated += this.OnWindowActivated;
    }

    public void OnWindowActivated() {
        LevelManager.Instance.ResumeLevel();
        this.SetDialogueToNextLineInScript();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (this.clickToContinue && allowToClick) {
                this.SetDialogueToNextLineInScript();
            }
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
        StartCoroutine(this.WaitForDialogue(this.dialogueBox.FadeDuration));
    }

    private IEnumerator WaitForDialogue(float duration) {
        float elapsedTime = 0;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            yield return null;
            this.clickToContinue = false;
        }
        this.clickToContinue = true;
    }

    private IEnumerator OnScriptAdvance() {
        float elapsedTime;
        switch (this.scriptIndex) {
            // indices start at 0
            case 0:
                yield return new WaitForSeconds(this.dialogueBox.FadeDuration * 5.0f / 3.0f);
                this.StartCoroutine(this.clickToContinueText.FadeIn());
                this.StartCoroutine(WaitForDialogue(this.clickToContinueText.FadeDuration));
                break;
            case 1:
                break;
            case 2:
                yield return new WaitForSeconds(this.dialogueBox.FadeDuration);
                this.pizzaRb.useGravity = true;
                break;
            case 3:
                break;
            case 4:
                yield return new WaitForSeconds(this.dialogueBox.FadeDuration);
                elapsedTime = 0;
                while (elapsedTime < this.dialogueBox.FadeDuration) {
                    float t = elapsedTime / this.dialogueBox.FadeDuration;
                    this.blackBG.color = Color.Lerp(Color.black, Color.clear, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                break;
            case 5:
                LevelManager.Instance.StartLevel();
                // remove pizza from the subcamera's culling mask
                this.subcam.cullingMask |= ~(1 << LayerMask.NameToLayer("Pizza"));
                this.StartCoroutine(this.clickToContinueText.FadeOut());
                this.allowToClick = false;
                break;
            case 7:
                yield return new WaitForSeconds(this.dialogueBox.FadeDuration);
                this.dialogueBox.fadeDuration = 0.2f; // VERY HACKY BUT IT WORKS BUT IS NOT PROPER OOP
                break;
            case 8:
                window.SetClickable(true);
                break;
            case 14:
            case 15:
                yield return new WaitForSeconds(3.5f);
                this.allowToClick = false;
                this.SetDialogueToNextLineInScript();
                break;
            case 16:
                window2.SetClickable(true);
                break;
            case 19:
                window3.SetClickable(true);
                break;
        }

        yield return null;
    }

    private void OnBeat(int beat) {
        // switch (beat) {
        //     case 2: // before 2nd jump
        //     case 7: // after 2nd jump, before 3rd jump
        //     case 15: // after freeze
        //     case 19:
        //         break;
        // }

        if (this.beatsToContinueTutorialOn.Contains(beat)) {
            SetDialogueToNextLineInScript();
        }
    } 

    private void OnFreeze(Collider other, bool flag) {
        if (!other.CompareTag("PizzaFeet")) return;
        if (flag) {
            this.SetDialogueToNextLineInScript();
        } else {
            LevelManager.Instance.PauseLevel();
        }

        this.SetDialogueToNextLineInScript();
    }

    private void OnFreeze(Collider other) {
        this.OnFreeze(other, false);
    }
}