using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player: MonoBehaviour
{
    CharacterController controller;
    float speed = 12f;
    float gravity = -9.81f * 2;
    float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isMoving;
    public AudioClip groundClip;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        CheckIfGrounded();
    }
    void CheckIfGrounded()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //resetting default velocity
        if(isGrounded && velocity.y < 0f )
        {
            velocity.y = -2f;
        }

        //getting the inputs
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        //Creating moving vector
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // check if player can jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            //going up
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //fallng back down
        velocity.y += gravity * Time.deltaTime;

        // Make player jump
        controller.Move(velocity * Time.deltaTime);

        if(lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        lastPosition = gameObject.transform.position;
    }
}
