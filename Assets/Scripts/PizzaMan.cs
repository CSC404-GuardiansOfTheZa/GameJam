using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaMan : MonoBehaviour {
    [SerializeField] private float jumpSpeed = 3.0f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private int jumpOnEveryNthBeat = 2;
    [Header("SFX")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] [Range(0, 1)] private float jumpSFXVolume = 0.8f;
    
    private Rigidbody rigidbody;
    private AudioSource asource;
    private Collider col;

    private float __fallMultiplier;
    private bool isJump = false;
    private Vector3 startPos;
    private float distanceToGround;

    public bool IsGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.2f);
    }

    public void JumpOnBeat(int beat){
        if (beat % jumpOnEveryNthBeat == 0){
            this.Jump();
        }
    }

    public void Jump() {
        if (IsGrounded()){
            isJump = true;
            asource.PlayOneShot(jumpSFX, jumpSFXVolume);
        }
    }

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        asource = GetComponent<AudioSource>();
        col = GetComponent<Collider>();
    }

    private void Start() {
        Conductor.Instance.onBeat += JumpOnBeat;
        startPos = transform.position;
        distanceToGround = Mathf.Abs(col.bounds.extents.y - col.bounds.center.y);
        Debug.Log(distanceToGround);
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
        }
        isJump = false;

        if (rigidbody.velocity.y < 0)
            verticalSpeed += Physics.gravity.y * (gravityMultiplier-1) * Time.fixedDeltaTime;
        
        rigidbody.velocity = Vector3.up * verticalSpeed;
    }
}
