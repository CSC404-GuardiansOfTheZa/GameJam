using UnityEngine;

public class JumpDebug : MonoBehaviour {

    [SerializeField]
    private GameObject jumpMarkerPrefab;

    [SerializeField]
    private Transform levelGeometry;

    [SerializeField]
    private int jumpBeatDivision = 2;

    [SerializeField]
    private float xOffset = 0.5f;

    void Start() {
        Conductor.Instance.onBeat += SpawnMarker;
    }

    void SpawnMarker(int beat) {
        if (beat % jumpBeatDivision == 1) return;
        GameObject g = Instantiate(jumpMarkerPrefab, new Vector3(xOffset, 0, 0), Quaternion.identity);
        g.transform.SetParent(levelGeometry);
    }
}
