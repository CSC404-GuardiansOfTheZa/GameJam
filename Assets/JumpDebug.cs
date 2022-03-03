using UnityEngine;

public class JumpDebug : MonoBehaviour {

    [SerializeField]
    private GameObject jumpMarkerPrefab;

    [SerializeField]
    private Transform levelGeometry;

    [SerializeField]
    private int jumpBeatDivision = 2;

    [SerializeField]
    private Vector3 offset = Vector3.zero;

    void Start() {
        Conductor.Instance.onBeat += SpawnMarker;
    }

    void SpawnMarker(int beat) {
        if (beat % jumpBeatDivision == 1) return;
        GameObject g = Instantiate(jumpMarkerPrefab, offset, Quaternion.identity);
        g.transform.SetParent(levelGeometry);
    }
}
