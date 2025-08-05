using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerMovementAdvanced : MonoBehaviour
{
    private PlayerControls controls;

    private Vector2 moveInput;
    private bool sprintPressed;
    private Vector2 inputVector;
    public Inventory inventary;

    [Header("Animation")]
    public Animator anim;

    [Header("Movement")]
    public float moveSpeed;

    [Header("Default")]
    public bool walkOn = false;
    public float walkSpeed = 10;
    public bool slowOn = false;
    public float slowSpeed = 5;

    [Header("Sprint")]
    public bool sprintOn = false;
    public float sprintSpeed = 15;
    [Space]
    public bool cd_sprint;
    public float sprinttime;
    public float maxsprinttime = 3;
    public GameObject SprintSlider;
    [Space]
    public float cd_sprinttime;
    public float maxcd_sprint = 3;
    public GameObject FatigueSlider;
    [Space]
    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("ref")]
    public Transform orientation;
    public CapsuleCollider capsuleCollider;

    public float horizontalInput;
    public float verticalInput;

    public Vector3 moveDirection;

    public Rigidbody rb;

    public static MovementState state;
    public enum MovementState
    {
        idle,
        walking,
        sprinting
    }
    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Gameplay.Sprint.performed += ctx => sprintPressed = true;
        controls.Gameplay.Sprint.canceled += ctx => sprintPressed = false;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
        controls.UI.Disable();
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;


        SprintSlider.SetActive(false);
        FatigueSlider.SetActive(false);

        sprinttime = maxsprinttime;
        cd_sprinttime = maxcd_sprint;
    }

    public void Update()
    {

        inputVector = controls.Gameplay.Move.ReadValue<Vector2>();

        // ground check
        grounded = Physics.Raycast(transform.position, new Vector3(0,-5,0), playerHeight * 0.5f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

    }

    private void OnDrawGizmos()
    {
        // Sp�cifie la couleur du Gizmo
        Gizmos.color = Color.red;

        // D�termine la longueur du Raycast
        float rayLength = playerHeight * 0.5f + 0.2f;

        // Dessine un rayon � partir de la position actuelle vers le bas
        Gizmos.DrawRay(transform.position, Vector3.down * rayLength);
    }

    public void FixedUpdate()
    {
        MovePlayer();
    }

    public void MyInput()
    {
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y;
    }

    public void StateHandler()
    {
        // Mode - Sprint
        if (grounded && walkSpeed > 0 && sprintSpeed > 0 && sprintPressed && moveInput != Vector2.zero)
        {
            sprinttime -= Time.deltaTime;

            if (sprinttime > 0)
            {
                SprintSlider.SetActive(true);
                moveSpeed = sprintSpeed;
                sprintOn = true;
            }

            if (sprinttime <= 0)
            {
                Debug.Log("fatigue");
                SprintSlider.SetActive(false);
                FatigueSlider.SetActive(true);
                sprinttime = 0;
                cd_sprinttime -= Time.deltaTime;
                moveSpeed = walkSpeed;
                cd_sprint = true;
                sprintOn = false;
            }

            if (cd_sprinttime < maxcd_sprint)
            {
                FatigueSlider.SetActive(true);
                cd_sprinttime += Time.deltaTime;
                Debug.Log("Non! Fatigu� !");
                if (cd_sprinttime > maxcd_sprint) cd_sprinttime = maxcd_sprint;
            }

            sprintOn = true;
            walkOn = false;
            slowOn = false;
            state = MovementState.sprinting;
            anim.SetFloat("speed", 1.5f, 0.1f, Time.deltaTime);
        }
        
        // Mode - Slow
        else if (grounded && walkSpeed > 0 && sprintSpeed > 0 && (inventary.isOpenCheckList || inventary.isOpenBag) && moveInput != Vector2.zero)
        {
            if (!(sprinttime <= 0))
            {
                SprintSlider.SetActive(true);
                FatigueSlider.SetActive(false);
                sprinttime += Time.deltaTime;
            }

            if (sprinttime > maxsprinttime)
            {

                SprintSlider.SetActive(false);
                FatigueSlider.SetActive(false);
                sprinttime = maxsprinttime;
            }

            if (sprinttime <= 0)
            {
                Debug.Log("fatigue");
                SprintSlider.SetActive(false);
                FatigueSlider.SetActive(true);
                sprinttime = 0;
                cd_sprinttime -= Time.deltaTime;
                moveSpeed = walkSpeed;
                cd_sprint = true;
                sprintOn = false;
            }

            if (cd_sprinttime <= 0)
            {
                Debug.Log("recover");
                SprintSlider.SetActive(true);
                FatigueSlider.SetActive(false);
                sprinttime = maxsprinttime;
                cd_sprinttime = maxcd_sprint;
                cd_sprint = false;
            }

            walkOn = false;
            sprintOn = false;
            slowOn = true;
            state = MovementState.walking;
            moveSpeed = slowSpeed;
            anim.SetFloat("speed", 0.5f, 0.1f, Time.deltaTime);
        }

        // Mode - Walk
        else if (grounded && walkSpeed > 0 && sprintSpeed > 0 && moveInput != Vector2.zero)
        {
            if (!(sprinttime <= 0))
            {
                SprintSlider.SetActive(true);
                FatigueSlider.SetActive(false);
                sprinttime += Time.deltaTime;
            }

            if (sprinttime > maxsprinttime)
            {

                SprintSlider.SetActive(false);
                FatigueSlider.SetActive(false);
                sprinttime = maxsprinttime;
            }

            if (sprinttime <= 0)
            {
                Debug.Log("fatigue");
                SprintSlider.SetActive(false);
                FatigueSlider.SetActive(true);
                sprinttime = 0;
                cd_sprinttime -= Time.deltaTime;
                moveSpeed = walkSpeed;
                cd_sprint = true;
                sprintOn = false;
            }

            if (cd_sprinttime <= 0)
            {
                Debug.Log("recover");
                SprintSlider.SetActive(true);
                FatigueSlider.SetActive(false);
                sprinttime = maxsprinttime;
                cd_sprinttime = maxcd_sprint;
                cd_sprint = false;
            }

            walkOn = true;
            sprintOn = false;
            slowOn = false;
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            anim.SetFloat("speed", 1f, 0.1f, Time.deltaTime);
        }

        // Mode - Idle
        else if (grounded)
        {
            if (!(sprinttime <= 0))
            {
                sprinttime += Time.deltaTime;
                SprintSlider.SetActive(true);
                FatigueSlider.SetActive(false);
            }

            if (sprinttime > maxsprinttime)
            {
                SprintSlider.SetActive(false);
                FatigueSlider.SetActive(false);
                sprinttime = maxsprinttime;
            }

            if (sprinttime <= 0)
            {
                Debug.Log("fatigue");
                SprintSlider.SetActive(false);
                FatigueSlider.SetActive(true);
                sprinttime = 0;
                cd_sprinttime -= Time.deltaTime;
                moveSpeed = 0;
                cd_sprint = true;
            }

            if (cd_sprinttime <= 0)
            {
                Debug.Log("recover");
                SprintSlider.SetActive(true);
                FatigueSlider.SetActive(false);
                sprinttime = maxsprinttime;
                cd_sprinttime = maxcd_sprint;
                cd_sprint = false;
            }

            walkOn = false;
            sprintOn = false;
            slowOn = false;
            state = MovementState.idle;
            moveSpeed = 0;
            anim.SetFloat("speed", 0f, 0.1f, Time.deltaTime);
        }
    }

    public void MovePlayer()
    {

        Vector3 moveInput = orientation.forward * inputVector.y + orientation.right * inputVector.x;
        moveDirection = moveInput.normalized;

        if (grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(Physics.gravity * rb.mass, ForceMode.Force);
        }
    }

    public void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}