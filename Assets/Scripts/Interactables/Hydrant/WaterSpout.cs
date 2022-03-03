using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpout : MonoBehaviour {
    [SerializeField] private ParticleSystem particleSystem;
    [Header("Spout Properties")]
    [SerializeField] private float secondsToExtend = 1.0f;
    [SerializeField] private float strengthOnPizzaGuy = 5.0f;

    public float SpoutHeight { get; set; } = -1;

    private bool extended;
    
    public void Awake() {
        SpoutHeight = transform.localScale.y;
        SetYScale(this.extended ? SpoutHeight : 0);
        EnableParticleSystem(this.extended);
    }

    public void ToggleSpout(bool _extended) {
        this.extended = _extended;
        EnableParticleSystem(this.extended);
        StartCoroutine(ExtendTo(_extended ? SpoutHeight : 0));
    }

    IEnumerator ExtendTo(float height) {
        float elapsedTime = 0.0f;
        while (elapsedTime < this.secondsToExtend) {
            float progress = elapsedTime / this.secondsToExtend;
            float newHeight = Mathf.Lerp(transform.localScale.y, height, progress);
            SetYScale(newHeight);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void EnableParticleSystem(bool flag) {
        if (this.particleSystem != null) {
            if (flag) {
                this.particleSystem.Play();
            } else {
                this.particleSystem.Stop();
            }
        }
    }
    
    private void SetYScale(float yScale) {
        // When setting scale, also set y position so it only scales from the top (instead of from the center)
        Vector3 scale = transform.localScale;
        scale.y = yScale;
        transform.localScale = scale;
        
        Vector3 pos = transform.localPosition;
        pos.y = yScale;
        transform.localPosition = pos;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Spout detects Player");
            PizzaMan pizzaMan = other.GetComponent<PizzaMan>() as PizzaMan;
            pizzaMan.ActivateWaterSpout(this.strengthOnPizzaGuy);
        }
    }
}
