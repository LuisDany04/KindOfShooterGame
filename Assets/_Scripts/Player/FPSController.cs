using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class FPSController : MonoBehaviour
{
    public float mouseSensitivity;
    public Transform body;
    public InputManager inputManager;
    public Rigidbody rb;



    //Mouse inputs
    private float mouseX;
    private float mouseY;
    //Movement inputs
    private float X;
    private float Y;
    private float xRotation = 0;

    [Header("Headbob Configuration")]
    public bool enableHeadbob = true;
    public float headbobAmplitude = 0.02f;
    public float headbobFrequency = 1.5f;
    public Transform cameraTransform;
    public Transform cameraOffsetTransform;
    public Transform headTransform;

    private Vector3 originalCameraPosition;
    [SerializeField] private float headbobTimer = 0f;
    private bool isMoving = false;

    private void Awake() {
        
    }

    private void Start() {
        //Cursor.lockState = CursorLockMode.Locked;
        originalCameraPosition = cameraTransform.localPosition;

    }

    private void Update() {
        CameraLook();
        playerMovement();
        UpdateHeadbob();
    }


    private void CameraLook() {
        //Reads the value of Mouse X and Y every frame
        mouseX = inputManager.inputMaster.CameraLook.MouseX.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
        mouseY = inputManager.inputMaster.CameraLook.MouseY.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
    
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        transform.localRotation = Quaternion.Euler(xRotation, -10f, 0f);
        body.Rotate(Vector3.up * mouseX);
    }

    private void CameraLookOff() { 

        //Reads the value of Mouse X and Y every frame
        mouseX = inputManager.inputMaster.CameraLook.MouseX.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
        mouseY = inputManager.inputMaster.CameraLook.MouseY.ReadValue<float>() * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        transform.localRotation = Quaternion.Euler(xRotation, -10f, 0f);
        body.Rotate(Vector3.up * mouseX);
    }

    private void playerMovement() {
        X = inputManager.inputMaster.Movement.X.ReadValue<float>();
        Y = inputManager.inputMaster.Movement.Y.ReadValue<float>();

        Vector3 move = transform.right * X + transform.forward * Y;

        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }

    private void UpdateHeadbob() {
        if (enableHeadbob && rb.velocity.magnitude > 0.1f) {
            float waveSlice = 0f;
            float horizontal = X;
            float vertical = Y;

            if (Mathf.Abs(horizontal) == 0f && Mathf.Abs(vertical) == 0f) {
                headbobTimer = 0f;
                Debug.Log("Headbob Timer = 0");
            } else {
                waveSlice = Mathf.Sin(headbobTimer);
                headbobTimer += headbobFrequency * Time.deltaTime;
                if (headbobTimer > Mathf.PI * 2) {
                    headbobTimer -= Mathf.PI * 2;
                }
            }

            if (waveSlice != 0) {
                float translateChange = waveSlice * headbobAmplitude;
                float totalAxes = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                float translateValue = totalAxes * translateChange;

                Vector3 newPosition = originalCameraPosition;
                newPosition.y = originalCameraPosition.y + translateValue;
                cameraTransform.localPosition = newPosition;

                // Simula el movimiento de la cabeza del jugador
                headTransform.localRotation = Quaternion.Euler(-translateValue * 10f, 0f, 0f);
            } else {
                cameraTransform.localPosition = originalCameraPosition;
                //The problem it's in here
                headTransform.localRotation = Quaternion.identity;
            }
        } else {
            cameraTransform.localPosition = originalCameraPosition;
            headTransform.localRotation = Quaternion.identity;
        }
    }
    
}
