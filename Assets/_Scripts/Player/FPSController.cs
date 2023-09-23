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

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        CameraLook();
        playerMovement();
        if (X != 0 || Y != 0) {
            BobbingEffect();
        }
    }


    private void CameraLook() {
        //Reads the value of Mouse X and Y every frame
        mouseX = inputManager.inputMaster.CameraLook.MouseX.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
        mouseY = inputManager.inputMaster.CameraLook.MouseY.ReadValue<float>() * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        body.Rotate(Vector3.up * mouseX);
    }

    private void playerMovement() {
        X = inputManager.inputMaster.Movement.X.ReadValue<float>();
        Y = inputManager.inputMaster.Movement.Y.ReadValue<float>();

        Vector3 move = transform.right * X + transform.forward * Y;

        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }

    private void BobbingEffect() {

    }
}
