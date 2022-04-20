using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PizzaMan : MonoBehaviour {
    [SerializeField] private float jumpSpeed = 3.0f;
    [SerializeField] private int jumpOnEveryNthBeat = 2;
    [Header("Grounding")]
    [SerializeField] private float groundingSensitivity = 0.2f;
    [SerializeField] private float groundingRaycastShift = 0.5f;
    [Header("SFX")]
    [SerializeField] private List<AudioClip> jumpSFX;
    [SerializeField][Range(0, 1)] private float jumpSFXVolume = 0.8f;
    [SerializeField] private AudioClip deathSFX;
    [Header("Children")]
    [SerializeField] private Animator modelAnimator;
    [SerializeField] private ParticleSystem onBeatMetronome;
    [SerializeField] private ParticleSystem offBeatMetronome;
    [SerializeField] private Transform model;
    [SerializeField] private GameObject ragdollPrefab;
    private Vector3 pauseVelocity;
    
    private static PizzaMan _instance;
    public static PizzaMan Instance { get { return _instance; } }
    
    public bool IsGrounded { get; set; }
    public int NumJumps { get; private set; }

    private Rigidbody rigidbody;
    private AudioSource asource;
    private Collider col;

    private float __fallMultiplier;
    private bool isJump = false;
    private Vector3 startPos;
    private float distanceToGround;
    private bool hitFireHydrant = false;
    private bool wasGrounded = false;
    private bool paused = false;
    private bool killed = false;
    private Vector3 pausePos;
    private GameObject ragdoll;

    // Event to be called everytime PizzaMan is grounded
    public event LevelManager.VoidDelegate OnGrounded;
    public event LevelManager.VoidDelegate OnKilled;

    public void Kill() {
        if (this.killed) return;
        
        this.killed = true;
        
        this.model.gameObject.SetActive(false);
        this.ragdoll = Instantiate(this.ragdollPrefab, transform) as GameObject;
        this.ragdoll.GetComponentInChildren<Rigidbody>()?.AddForce(new Vector3(0, 0.2f, 1) * 200f, ForceMode.Impulse);
        
        this.asource.PlayOneShot(this.deathSFX);
        
        OnKilled?.Invoke();
    }

    public void Respawn() {
        this.model.gameObject.SetActive(true);
        Destroy(this.ragdoll);
        this.killed = false;        
        transform.position = this.startPos;
    }

    public void ActivateWaterSpout(float strength) {
        // Called when makes contact with the water spout from a fire hydrant
        this.hitFireHydrant = true;
        this.rigidbody.AddForce(Vector3.up * strength);
        Debug.Log("Player recieved function call from Spout!");
    }

    public void OnPause() {
        this.paused = true;
        this.pausePos = transform.position;
        this.rigidbody.useGravity = false;
        pauseVelocity = this.rigidbody.velocity;
        this.rigidbody.velocity = Vector3.zero;
        this.modelAnimator.speed = 0;
        this.onBeatMetronome.Pause();
        this.offBeatMetronome.Pause();
    }

    public void OnResume() {
        this.paused = false;
        this.rigidbody.useGravity = true;
        this.rigidbody.velocity = pauseVelocity;
        this.modelAnimator.speed = 1.0f;
        this.onBeatMetronome.Play();
        this.offBeatMetronome.Play();
    }

    public void SetNextSpawnToCurrentPos() {
        this.startPos = transform.position;
    }
    
    /// //////////////////////////////////////////////////////////////////

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
    private void JumpOnBeat(int beat) {
        this.onBeatMetronome.Emit(1);
        if (beat % jumpOnEveryNthBeat == 0) {
            this.Jump();
        }
    }

    private void Jump() {
        if (!this.killed && !this.isJump && IsGrounded) {
            isJump = true;
            asource.PlayOneShot(jumpSFX[UnityEngine.Random.Range(0, this.jumpSFX.Count)], jumpSFXVolume);
            NumJumps++;
            this.modelAnimator.SetTrigger(Jump_Anim);
        } else {
            Debug.Log("couldn't jump because I'm not grounded!");
        }
    }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        
        rigidbody = GetComponent<Rigidbody>();
        asource = GetComponent<AudioSource>();
        col = GetComponent<Collider>();
    }

    private void Start() {
        if (Conductor.Instance != null) {
            Conductor.Instance.onBeat += JumpOnBeat;
            Conductor.Instance.onOffBeat += delegate(int num) { this.offBeatMetronome.Emit(1); }; 
        } else {
            Debug.LogWarning("Pizza Guy could not find conductor, and can not connect to the beat");
        }
        startPos = transform.position;
        distanceToGround = Mathf.Abs(col.bounds.extents.y - col.bounds.center.y);
        IsGrounded = false;
        LevelManager.Instance.OnPause += this.OnPause;
        LevelManager.Instance.OnResume += this.OnResume;
        LevelManager.Instance.OnLevelReload += this.Respawn;
    }

    private int __FRAME = 0;
    private static readonly int IsGrounded_Anim = Animator.StringToHash("IsGrounded");
    private static readonly int Jump_Anim = Animator.StringToHash("Jump");

    private void Update() {
        if (this.paused) {
            // transform.position = this.pausePos;
            return;
        }

// #if UNITY_EDITOR
//         if (Input.GetButtonDown("Jump")) {
//             Jump();
//         }
// #endif
         // lock pizzaguy into a set path
        transform.position = new Vector3(
            startPos.x,
            transform.position.y,
            startPos.z
        );

        IsGrounded = this.CheckIfGrounded();
        this.modelAnimator.SetBool(IsGrounded_Anim, IsGrounded);

        if (IsGrounded) {
            // Debug.Log($"Grounded on frame {this.__FRAME}");
            if (!this.wasGrounded && this.OnGrounded != null) {
                // became grounded this frame
                this.OnGrounded();
            }
        } else {
            // Debug.Log($"Airborne on frame {this.__FRAME}");
        }

        this.wasGrounded = IsGrounded;
        this.__FRAME++;
    }


    private void FixedUpdate() {
        if (this.paused) return;

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
