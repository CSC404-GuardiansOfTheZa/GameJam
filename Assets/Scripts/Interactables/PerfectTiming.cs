using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TimingClassData {
    public string name;
    public Sprite sprite;
    public float toleranceInBeats;
    public AudioClip audio;
    public Color debugColor;
}

public class PerfectTiming : MonoBehaviour
{
    // These fields are public so we can have a custom editor for them
    [SerializeField] private bool areTimingsEnabled = true;
    [SerializeField] private Interactable parentInteractable;
    [Header("Sprite")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float unitsSpriteRises = 1.5f;
    [SerializeField] private float spriteDuration = 2.0f;
    [Range(0, 1)] [SerializeField] private float whenToFadeSprite = 0.5f;
    [SerializeField] private Vector3 scalingFactor = 2 * Vector3.one;

    [Header("Tolerances")] 
    [SerializeField] private bool isYToleranceAbove;
    [SerializeField] private TimingClassData yTolerance;
    [SerializeField] private List<TimingClassData> tolerances; 

    private bool hasBeenTriggered;
    private AudioSource asource;
    private float beatShouldBeActivatedOn = -2.0f;
    private float distancePerBeat;

    private void Start() {
        this.asource = this.GetComponent<AudioSource>();
        this.sprite.gameObject.SetActive(false);
        
        float scrollSpeed = LevelManager.Instance.gameObject.GetComponent<Scroller>().scrollSpeed;
        float crotchet = Conductor.Instance.Crotchet;
        this.distancePerBeat = scrollSpeed * crotchet;
        this.beatShouldBeActivatedOn = (transform.position.x - PizzaMan.Instance.transform.position.x) / distancePerBeat; // assumes pizza guy starts at x=0
        
        parentInteractable.OnTrigger += this.OnTrigger;
        LevelManager.Instance.OnLevelReload += delegate { this.hasBeenTriggered = false; };
    }

    private void OnTrigger() {
        if (this.hasBeenTriggered || this.beatShouldBeActivatedOn <= 0) return;
        this.hasBeenTriggered = true;
        
        // first, check the y-threshold
        float pizzaY = PizzaMan.Instance.transform.position.y;
        float yThreshold = transform.position.y + this.yTolerance.toleranceInBeats;
        if ((!this.isYToleranceAbove && pizzaY < yThreshold) || (this.isYToleranceAbove && pizzaY > yThreshold)) {
            TriggerTolerance(this.yTolerance);
            return;
        }

        Debug.LogFormat("CheckingTimings: {0}", this.beatShouldBeActivatedOn);
        float triggeredBeat = Conductor.Instance.SongPositionInBeats;
        float diff = this.beatShouldBeActivatedOn - triggeredBeat; // positive ==> early, negative ==> late

        foreach (var tolerance in this.tolerances) {
            if (diff < tolerance.toleranceInBeats) {
                TriggerTolerance(tolerance); 
                return;
            }
        }
    }

    private void TriggerTolerance(TimingClassData tolerance) {
        Debug.Log(tolerance.name);
        
        if (tolerance.audio is not null)
            this.asource.PlayOneShot(tolerance.audio, 1.0f);
        
        StartCoroutine(this.ShowSprite(tolerance.sprite));
    }

    private IEnumerator ShowSprite(Sprite spriteToShow) {
        // ensure scaling is fine:
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3 (
            this.scalingFactor.x/transform.lossyScale.x, 
            this.scalingFactor.y/transform.lossyScale.y, 
            this.scalingFactor.z/transform.lossyScale.z
        ); 
        
        this.sprite.gameObject.SetActive(true);
        this.sprite.sprite = spriteToShow;

        Vector3 spritePos = this.sprite.transform.localPosition;
        float startingY = spritePos.y;
        float targetY = startingY + this.unitsSpriteRises;

        float progress = 0.0f;
        while (progress < 1.0f) {
            float positionT = EaseOutExpo(progress);
            spritePos.y = Mathf.Lerp(startingY, targetY, positionT);
            this.sprite.transform.localPosition = spritePos;

            float alphaT = DelayedLinear(progress, this.whenToFadeSprite);
            sprite.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, alphaT));
            
            progress += Time.deltaTime / this.spriteDuration;
            yield return null;
        }

        spritePos.y = startingY;
        this.sprite.transform.localPosition = spritePos;
        this.sprite.gameObject.SetActive(false);
    }

    private float EaseOutExpo(float t) {
        // given a linear value t, from 0 to 1, ease out expo
        return Mathf.Approximately(t, 1) ? 1 : 1 - Mathf.Pow(2, -10 * t);
    }

    private float DelayedLinear(float t, float threshold) {
        return t < threshold ? 0 : (1 / (1 - threshold)) * (t - threshold);
    }

    private void OnDrawGizmosSelected() {
        Vector3 pos = transform.position;
        
        // draw y threshold
        float y = pos.y + this.yTolerance.toleranceInBeats;
        Gizmos.color = this.yTolerance.debugColor;
        Gizmos.DrawLine(new Vector3(pos.x - 5, y, pos.z), new Vector3(pos.x + 5, y, pos.z));

        // draw each x threshold
        Gizmos.color = Color.yellow;
        foreach (var tolerance in this.tolerances) {
            float dist = tolerance.toleranceInBeats * this.distancePerBeat;
            Gizmos.color = tolerance.debugColor;
            Gizmos.DrawLine(
                new Vector3(pos.x - dist, y, pos.z), 
                new Vector3(pos.x - dist, y + (this.isYToleranceAbove ? -15 : 15), pos.z)
            );
        }
    }
}
