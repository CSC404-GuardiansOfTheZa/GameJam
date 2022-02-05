using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject treeToSpawn;
    [SerializeField]
    private BeatKeeper beatKeeper;

    [Space(5)]
    [Header("Children")]
    [SerializeField]
    private Transform dynamicChildren;
    [SerializeField]
    private Transform road;
    [SerializeField]
    private Transform endMarker;

    [Space(5)]
    [Header("Level Info")]
    [SerializeField]
    private float roadLength = 20;

    [Space(5)]
    [Header("Audio")]
    [SerializeField]
    private float trackLengthInSeconds;
    [SerializeField]
    private int trackBPM;
    [SerializeField]
    private int everyNthBeat = 2; // e.g. if 4, then will jump every 4th beat.

    [Space(5)]
    [Header("Pizza and Co")]
    [SerializeField]
    private PizzaMan pizzaMan;


    void Awake()
    {
        dynamicChildren.GetComponent<Scrolling>().Init(trackLengthInSeconds, roadLength);
    }

    void Start()
    {
        road.localScale = new Vector3(roadLength, 1, 1);
        road.localPosition = Vector3.right * roadLength * 5;
        endMarker.localPosition = Vector3.right * roadLength * 10;

        float secondsPerBeat = 60.0f/(float)trackBPM;
        float poleIntervals = ((roadLength * 10) / trackLengthInSeconds) * secondsPerBeat * everyNthBeat; // scroll speed (units/s) * # of s / beat
        for (int i = 0; i < everyNthBeat * (int) ((trackLengthInSeconds/60.0f) * trackBPM); i++){
            Instantiate(treeToSpawn, new Vector3(i * poleIntervals, 2, 4), Quaternion.identity, dynamicChildren);
        }

        StartCoroutine(Beat(secondsPerBeat * everyNthBeat));
    }

    void Update() {
        if (pizzaMan.transform.position.y < -30) {
            Debug.Log("LOSE LOSE LOSE LOSE!");
            dynamicChildren.GetComponent<Scrolling>().Stop();
        }

        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator Beat(float beatTime) {
        while (true){
            this.beatKeeper.Beat();
            float jumpSpeed = Random.Range(3, 4);
            this.pizzaMan.Jump(jumpSpeed);
            yield return new WaitForSeconds(beatTime);
        }
    }

}
