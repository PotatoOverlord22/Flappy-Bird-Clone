using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float _speed;
    private const float TOLERANCE = 0.001f;
    public float speed
    {
        get { return _speed; }
        set
        {
            if (Mathf.Abs(_speed - value) < TOLERANCE)
                return;
            rb.velocity = new Vector2(-_speed, 0);
        }
    }

    private void Awake()
    {
        if (rb == null)
            rb = this.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = new Vector2(-_speed, 0);
    }
}