using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    private PlayerControls controls;

    [SerializeField] private Vector2 moveInput;
    [SerializeField] private bool sprintPressed;
    private Vector2 inputVector;
    public Transform orientation;
    public Vector3 moveDirection;


    //[SerializeField] private Animator animator; 
    //[SerializeField] private Rigidbody rbody;


    [Header("Design")]
    public float Speed;
    public bool isWalking;
    public float Walk;
    public bool isRunning;
    public float Run;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Gameplay.Sprint.performed += ctx => sprintPressed = true;
        controls.Gameplay.Sprint.canceled += ctx => sprintPressed = false;
    }


    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Vector3 moveInput = orientation.forward * inputVector.y + orientation.right * inputVector.x;
        //moveDirection = moveInput.normalized;
        characterController.Move(move * Time.deltaTime*Speed);
        //inputVector = controls.Gameplay.Move.ReadValue<Vector2>();
    }
}
