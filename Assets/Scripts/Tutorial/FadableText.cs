using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FadableText : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1.0f;

    public float FadeDuration => this.fadeDuration;

    protected Color startColor;
    protected TextMeshProUGUI tmp;
    
    // Start is called before the first frame update
    protected void Start() {
        this.tmp = this.GetComponent<TextMeshProUGUI>();
        this.startColor = this.tmp.color;
    }

    public IEnumerator SetText(string text, Color newTextColor) {
        if (!ColorUtils.Equals(this.tmp.color, Color.clear)) {
            yield return StartCoroutine(FadeAway());
        }
        
        this.tmp.text = text;
        yield return StartCoroutine(Fade(this.fadeDuration, Color.clear, newTextColor));
    }

    public IEnumerator SetText(string text) {
        yield return StartCoroutine(SetText(text, this.startColor));
    }

    public IEnumerator FadeAway() {
        yield return StartCoroutine(Fade(this.fadeDuration, this.tmp.color, Color.clear));
    }

    private IEnumerator Fade(float duration, Color startColor, Color endColor) {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            this.tmp.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
