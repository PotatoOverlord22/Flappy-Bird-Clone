using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private const float TOLERANCE = 0.001f;
    [SerializeField] private float _pipeSpeed;
    public float pipeSpeed
    {
        get { return _pipeSpeed; }
        set
        {
            if (Mathf.Abs(_pipeSpeed - value) < TOLERANCE) return;
            _pipeSpeed = value;
            rb.velocity = new Vector2(-_pipeSpeed, 0);
        }
    }

    private void Start()
    {
        rb.velocity = new Vector2(-pipeSpeed, 0);
    }
}