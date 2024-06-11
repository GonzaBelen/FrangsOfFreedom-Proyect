using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private Rigidbody2D rb2D;    

    [Header("Movement")]
    public float movementSpeed;

    [Header("Jump")]
    public float jumpForce;

    [Header("Attack")]
    public float attackRange = 2f;

    [Header("Dash")]
    public float dashForce;
    public float dashingTime;
    public float coolDown;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void ChangeGravity(float value)
    {
        rb2D.gravityScale = value;
    }
}
