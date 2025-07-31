using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

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


    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(move*Time.deltaTime*Speed);
    }
}
