using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FadableText : MonoBehaviour
{
    [SerializeField] public float fadeDuration = 1.0f;

    public float FadeDuration => this.fadeDuration;

    protected Color startColor;
    protected TextMeshProUGUI textField;
    
    // Start is called before the first frame update
    protected void Start() {
        this.textField = this.GetComponent<TextMeshProUGUI>();
        this.startColor = this.textField.color;
    }

    public IEnumerator SetText(string text, Color newTextColor) {
        text = text.Replace("\\n", "\n");
        if (this.textField != null) {
            if (!ColorUtils.Equals(this.textField.color, Color.clear)) {
                yield return StartCoroutine(this.FadeOut());
            }

            this.textField.SetText(text);
            this.textField.parseCtrlCharacters = true;
            yield return StartCoroutine(Fade(this.fadeDuration, Color.clear, newTextColor));
        }
    }

    public IEnumerator SetText(string text) {
        yield return StartCoroutine(SetText(text, this.startColor));
    }

    public IEnumerator FadeOut() {
        yield return StartCoroutine(Fade(this.fadeDuration, this.textField.color, Color.clear));
    }

    public IEnumerator FadeIn() {
        yield return StartCoroutine(Fade(this.fadeDuration, Color.clear, this.startColor));
    }

    private IEnumerator Fade(float duration, Color startColor, Color endColor) {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            this.textField.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
