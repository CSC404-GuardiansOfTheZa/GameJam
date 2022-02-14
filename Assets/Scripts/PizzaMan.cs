using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaMan : MonoBehaviour {
    [SerializeField] private float jumpSpeed = 3.0f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private int jumpOnEveryNthBeat = 2;
    [SerializeField] private float groundingSensitivity = 0.2f;
    [Header("SFX")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] [Range(0, 1)] private float jumpSFXVolume = 0.8f;

    public bool IsGrounded {
        get;  private set;
    }

    private Rigidbody rb;
    private AudioSource audioSrc;
    private Collider col;

    private float fallMultiplier;
    private bool isJumping = false;
    private Vector3 startPos;
    private float distanceToGround; // READONLY; this is the distance from the object's pivot to the ground, when standing on the ground.

    public bool GroundedCheck() {
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround + groundingSensitivity);
    }

    public void CheckIfCanJumpOnBeat(int beat){
        if (beat % jumpOnEveryNthBeat == 0){
            this.Jump();
        }
    }

    public void Jump() {
        this.isJumping = true;
    }

    private void Awake() {
        this.rb = GetComponent<Rigidbody>();
        this.audioSrc = GetComponent<AudioSource>();
        this.col = GetComponent<Collider>();
    }

    private void Start() {
        Conductor.Instance.onBeat += this.CheckIfCanJumpOnBeat;
        startPos = transform.position;
        distanceToGround = Mathf.Abs(col.bounds.extents.y - col.bounds.center.y);
    }

    private void Update() {
        #if UNITY_EDITOR
        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
        #endif
    
        // make sure PizzaGuy stays on the right z track
        transform.position = new Vector3(
            startPos.x, // TODO: check if this causes issues with colliding with walls???? prob not if instant kills.
            transform.position.y,
            startPos.z
        );
    }
    
    private void FixedUpdate() {
        this.IsGrounded = GroundedCheck();
        
        float verticalSpeed = this.rb.velocity.y;
        if (this.isJumping && this.IsGrounded) {
            verticalSpeed = jumpSpeed;
            this.audioSrc.PlayOneShot(jumpSFX, jumpSFXVolume);
            this.isJumping = false;
        }

        if (this.rb.velocity.y < 0)
            verticalSpeed += Physics.gravity.y * (gravityMultiplier-1) * Time.fixedDeltaTime;
        
        this.rb.velocity = Vector3.up * verticalSpeed;
    }
}
