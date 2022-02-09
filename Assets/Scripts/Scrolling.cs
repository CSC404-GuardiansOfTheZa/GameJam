using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    
    private float progress = 0; // 0 to 1
    private bool isInitialized = false;
    private float trackLength; // in seconds
    private float roadLength; // based on localScale.z of the road object

    public void Init(float trackLength, float roadLength) {
        this.trackLength = trackLength;
        this.roadLength = roadLength;
        this.isInitialized = true;
        this.progress = 0;
    }

    public void Stop()
    {
        isInitialized = false;
    }

    void Update()
    {
        if (isInitialized && progress < 1.0f){
            progress += Time.deltaTime / trackLength;
            this.transform.localPosition = Vector3.left * progress * (roadLength * 10);
        }
    }
}
