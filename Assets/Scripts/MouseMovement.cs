using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    private CharacterController controller;
    float mouseSensitivity = 500f;
    float xRotation = 0f;
    float yRotation = 0f;
    float topClamp = -90f;
    float bottomClamp = 90f;
    // Start is called before the first frame update
    void Start()
    {
        //Lock mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MousePosition();
    }

    void MousePosition()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Look up and down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp); //clamp rotation

        yRotation += mouseX; // Look left and right
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f); // Apply rotations
    }
}
