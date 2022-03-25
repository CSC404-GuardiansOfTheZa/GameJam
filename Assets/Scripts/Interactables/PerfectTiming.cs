using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderGraph;
using UnityEngine;

[System.Serializable]
public class TimingClassData {
    public string name;
    public Sprite sprite;
    public float toleranceInBeats;
    public AudioClip audio;
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
    [SerializeField] private List<TimingClassData> tolerances; 

    private bool hasBeenTriggered;
    private AudioSource asource;
    private float beatShouldBeActivatedOn = -2.0f;

    private void Start() {
        this.asource = this.GetComponent<AudioSource>();
        this.sprite.gameObject.SetActive(false);
        
        float scrollSpeed = LevelManager.Instance.gameObject.GetComponent<Scroller>().scrollSpeed;
        float crotchet = Conductor.Instance.Crotchet;
        float distancePerBeat = scrollSpeed * crotchet;
        this.beatShouldBeActivatedOn = (transform.position.x - PizzaMan.Instance.transform.position.x) / distancePerBeat; // assumes pizza guy starts at x=0
        
        parentInteractable.OnTrigger += this.OnTrigger;
        LevelManager.Instance.OnLevelReload += delegate { this.hasBeenTriggered = false; };
    }

    private void OnTrigger() {
        if (this.hasBeenTriggered || this.beatShouldBeActivatedOn <= 0) return;
        this.hasBeenTriggered = true;
        
        Debug.LogFormat("CheckingTimings: {0}", this.beatShouldBeActivatedOn);
        float triggeredBeat = Conductor.Instance.SongPositionInBeats;
        float diff = this.beatShouldBeActivatedOn - triggeredBeat; // positive ==> early, negative ==> late

        foreach (var tolerance in this.tolerances) {
            if (diff < tolerance.toleranceInBeats) {
                Debug.Log(tolerance.name);
                if (tolerance.audio is not null)
                    this.asource.PlayOneShot(tolerance.audio, 1.0f);
                else 
                    Debug.Log("AAAAAAAAAAAAAAAAAAAA");

                StartCoroutine(this.ShowSprite(tolerance.sprite));
                break;
            }
        }
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
}
