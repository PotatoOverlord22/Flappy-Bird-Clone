using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlappyBirdMovement : MonoBehaviour
{
    private FlappyBirdInputs input;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public float jumpForce;
    [SerializeField] private float rotationSpeed;

    private void Awake()
    {
        if (input == null)
            input = new FlappyBirdInputs();
        if (rb == null)
            rb = this.GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        input.Enable();
        input.FlappyBird.Flap.performed += OnFlapPerformed;
    }

    private void OnDisable()
    {
        input.FlappyBird.Flap.performed -= OnFlapPerformed;
        input.Disable();
    }

    private void OnFlapPerformed(InputAction.CallbackContext isPressed)
    {
        rb.velocity = new Vector2(0, jumpForce);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0,0, rb.velocity.y * rotationSpeed);
    }
}