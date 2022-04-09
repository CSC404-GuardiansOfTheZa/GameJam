using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Outline : MonoBehaviour {
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineScaleFactor;
    [SerializeField] private Color outlineColor;
    private GameObject outlineObject;

    [SerializeField]
    private Color highlightColor = Color.yellow;

    private Color[] materialColors;
    private Renderer[] childrenRenderers;


    async void Start() {
        // outlineObject = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
        // outlineObject.SetActive(false);
        childrenRenderers = GetComponentsInChildren<MeshRenderer>();
        childrenRenderers = this.childrenRenderers.Where((rnd => !rnd.CompareTag("TextMesh"))).ToArray();
        
        materialColors = new Color[childrenRenderers.Length];
        
        for (int i = 0; i < childrenRenderers.Length; i++) {
            materialColors[i] = childrenRenderers[i].material.color;
        }
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
        foreach (var renderer in childrenRenderers) {
            renderer.material.color = highlightColor;
        }
        // outlineObject.SetActive(true);
    }

    public void HideOutline() {
        for (int i = 0; i < childrenRenderers.Length; i++) {
            childrenRenderers[i].material.color = materialColors[i];
        }
        // outlineObject.SetActive(false);
    }

}