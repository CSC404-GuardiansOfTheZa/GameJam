using System;
using System.Collections.Generic;
using UnityEngine;

public class PizzaMan : MonoBehaviour {
    [SerializeField] private float jumpSpeed = 3.0f;
    [SerializeField] private int jumpOnEveryNthBeat = 2;
    [Header("Grounding")]
    [SerializeField] private float groundingSensitivity = 0.2f;
    [SerializeField] private float groundingRaycastShift = 0.5f;
    [Header("SFX")]
    [SerializeField] private List<AudioClip> jumpSFX;
    [SerializeField] [Range(0, 1)] private float jumpSFXVolume = 0.8f;
    
    public bool IsGrounded { get; set; }

    private Rigidbody rigidbody;
    private AudioSource asource;
    private Collider col;

    private float __fallMultiplier;
    private bool isJump = false;
    private Vector3 startPos;
    private float distanceToGround;
    private bool hitFireHydrant = false;
    private bool wasGrounded = false;
    
    // Event to be called everytime PizzaMan is grounded
    public delegate void onGroundedDelegate();
    public event onGroundedDelegate onGrounded;

    public void ActivateWaterSpout(float strength) {
        // Called when makes contact with the water spout from a fire hydrant
        this.hitFireHydrant = true;
        this.rigidbody.AddForce(Vector3.up * strength);
        Debug.Log("Player recieved function call from Spout!");
    }
    
    private bool CheckIfGrounded() {
        // int layerMask = Physics.DefaultRaycastLayers & ~LayerMask.GetMask("Pizza");
        int layerMask = LayerMask.GetMask("Platforms");
        // Shoot the raycast from one unit up: this helps pre
        return Physics.Raycast(
            transform.position + (groundingRaycastShift * Vector3.up), 
            Vector3.down, 
            distanceToGround + groundingSensitivity + groundingRaycastShift, 
            layerMask
        );
    }
    private void JumpOnBeat(int beat){
        if (beat % jumpOnEveryNthBeat == 0){
            this.Jump();
        }
    }

    private void Jump() {
        if (IsGrounded){
            isJump = true;
            asource.PlayOneShot(jumpSFX[UnityEngine.Random.Range(0, this.jumpSFX.Count)], jumpSFXVolume);
        } else {
            Debug.Log("couldn't jump because I'm not grounded!");
        }
    }

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        asource = GetComponent<AudioSource>();
        col = GetComponent<Collider>();
    }


    private void Start() {
        if (Conductor.Instance != null) {
            Debug.LogWarning("Pizza Guy could not find conductor, and can not connect to the beat");
            Conductor.Instance.onBeat += JumpOnBeat;
        }
        startPos = transform.position;
        distanceToGround = Mathf.Abs(col.bounds.extents.y - col.bounds.center.y);
        IsGrounded = false;
    }

    private int __FRAME = 0;
    private void Update() {
#if UNITY_EDITOR
        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
#endif
        // lock pizzaguy into a set path
        transform.position = new Vector3(
            startPos.x,
            transform.position.y,
            startPos.z
        );

        IsGrounded = this.CheckIfGrounded();

        if (IsGrounded) {
            Debug.Log($"Grounded on frame {this.__FRAME}");
            if (!this.wasGrounded && onGrounded != null) {
                // became grounded this frame
                onGrounded();
            }
        } else {
            Debug.Log($"Airborne on frame {this.__FRAME}");
        }
        
        this.wasGrounded = IsGrounded;
        this.__FRAME++;
    }


    private void FixedUpdate() {
        float verticalSpeed = rigidbody.velocity.y;

        if (isJump) {
            verticalSpeed = jumpSpeed;
            isJump = false;
        }

        if (rigidbody.velocity.y < 0)
            verticalSpeed += Physics.gravity.y * Time.fixedDeltaTime;

        if (this.hitFireHydrant) {
            // if (this.rigidbody.velocity.y < 0) verticalSpeed /= 2;
            // else verticalSpeed *= 2;
            this.hitFireHydrant = false;
        }

        rigidbody.velocity = Vector3.up * verticalSpeed;
        // print(verticalSpeed);
    }

    private void OnDrawGizmos() {
        Vector3 currentPos = transform.position;
        Vector3 from = currentPos;
        from.y += this.groundingRaycastShift;
        Vector3 until = currentPos;
        until.y -= (this.groundingSensitivity + this.distanceToGround);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(from, until);
    }
}
