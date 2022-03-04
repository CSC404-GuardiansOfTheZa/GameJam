using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour {
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineScaleFactor;
    [SerializeField] private Color outlineColor;
    private GameObject outlineObject;


    void Start() {
        // outlineObject = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
        // outlineObject.SetActive(false);
    }

    GameObject CreateOutline(Material outlineMat, float scaleFactor, Color color) {

        GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        outlineObject.transform.localScale = Vector3.one;
        outlineObject.transform.localPosition = new Vector3(0, 0, 0);

        foreach (var rend in outlineObject.GetComponentsInChildren<Renderer>()) {
            rend.material = outlineMat;
            rend.material.SetColor("_OutlineColor", color);
            rend.material.SetFloat("_ScaleFactor", scaleFactor);
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            // rend.enabled = false;
        }


        foreach (var outline in outlineObject.GetComponentsInChildren<Outline>())
            outline.enabled = false;
        foreach (var collider in outlineObject.GetComponentsInChildren<Collider>())
            collider.enabled = false;

        return outlineObject;
    }

    public void ShowOutline() {
        // outlineObject.SetActive(true);
        foreach (var renderer in GetComponentsInChildren<Renderer>()) {
            // renderer.material = outlineMaterial;
            renderer.material.color = Color.yellow;
        }
    }

    public void HideOutline() {
        foreach (var renderer in GetComponentsInChildren<Renderer>()) {
            // renderer.material = outlineMaterial;
            renderer.material.color = Color.white;
        }
        // outlineObject.SetActive(false);
    }

}