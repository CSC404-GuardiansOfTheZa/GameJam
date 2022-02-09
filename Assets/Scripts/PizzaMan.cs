using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaMan : MonoBehaviour {
    [SerializeField] private float jumpSpeed = 3.0f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [Header("SFX")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] [Range(0, 1)] private float jumpSFXVolume = 0.8f;

    private float __fallMultiplier;
    private bool isJump = false;
    private Rigidbody rigidbody;
    private AudioSource asource;
    private Vector3 startPos;


    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        asource = GetComponent<AudioSource>();
    }

    private void Start() {
        Conductor.Instance.onBeat += JumpOnBeat;
        startPos = transform.position;
    }

    private void Update() {
        #if UNITY_EDITOR
        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
        #endif

        transform.position = new Vector3(
            startPos.x,
            transform.position.y,
            startPos.z
        );
    }


    private void FixedUpdate() {
        float verticalSpeed = rigidbody.velocity.y;

        if (isJump) {
            verticalSpeed = jumpSpeed;
            isJump = false;
        }

        if (rigidbody.velocity.y < 0)
            verticalSpeed += Physics.gravity.y * (gravityMultiplier-1) * Time.fixedDeltaTime;
        
        rigidbody.velocity = Vector3.up * verticalSpeed;
    }

    public void JumpOnBeat(int beat){
        if (beat % 2 == 0){
            this.Jump();
        }
    }

    public void Jump() {
        // TODO: shoot raycast down to check if actually on the ground.
        isJump = true;
        asource.PlayOneShot(jumpSFX, jumpSFXVolume);
    }
}
