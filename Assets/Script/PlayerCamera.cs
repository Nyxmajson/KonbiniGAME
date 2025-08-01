using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    private Vector2 inputVector;
    private PlayerControls controls;

    public float rotationSpeed;

    public CameraStyle currentStyle;

    public GameObject BasicCam;
    public GameObject CinematicCam;
    public GameObject Camera1Cam;
    public GameObject Camera2Cam;
    public GameObject Camera3Cam;

    [Header("R�f�rence � la Cinemachine FreeLook")]
    public CinemachineFreeLook freeLookCam;

    public enum CameraStyle
    {
        Basic,
        Cinematic,
        Camera1,
        Camera2,
        Camera3
    }

    private void Start()
    {
        /*Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Update()
    {


        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // rotate player object
        if (currentStyle == CameraStyle.Basic)
        {
            //inputVector = controls.Gameplay.Move.ReadValue<Vector2>();
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                inputDir.y = 0;
                inputDir.Normalize();

                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir, Time.deltaTime * rotationSpeed);
            }
        }

        Vector3 euler = playerObj.eulerAngles;
        playerObj.eulerAngles = new Vector3(0, euler.y, 0);
    }

    public void SwitchCameraStyle(CameraStyle newStyle)
    {
        CinematicCam.SetActive(false);
        Camera1Cam.SetActive(false);
        Camera2Cam.SetActive(false);
        Camera3Cam.SetActive(false);

        if (newStyle == CameraStyle.Basic) BasicCam.SetActive(true);
        if (newStyle == CameraStyle.Cinematic) CinematicCam.SetActive(true);
        if (newStyle == CameraStyle.Camera1) Camera1Cam.SetActive(true);
        if (newStyle == CameraStyle.Camera2) Camera2Cam.SetActive(true);
        if (newStyle == CameraStyle.Camera3) Camera3Cam.SetActive(true);

        currentStyle = newStyle;
    }

    public void SetFreeLookParameters(
        float xAxisValue,
        float fieldOfView,
        float screenX = 0.5f,
        float screenY = 0.5f
    )
    {
        if (freeLookCam == null)
        {
            Debug.LogError("Cinemachine FreeLook non assign�e !");
            return;
        }

        freeLookCam.m_XAxis.Value = xAxisValue;

        freeLookCam.m_Lens.FieldOfView = fieldOfView;

        for (int i = 0; i < 3; i++)
        {
            var rig = freeLookCam.GetRig(i);
            if (rig != null)
            {
                var composer = rig.GetComponentInChildren<CinemachineComposer>();
                if (composer != null)
                {
                    composer.m_ScreenX = screenX;
                    composer.m_ScreenY = screenY;
                }
            }
        }
    }

}
