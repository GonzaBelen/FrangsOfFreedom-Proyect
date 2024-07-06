using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesMovement : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private Transform groundController;
    [SerializeField] private float distance;
    [SerializeField] private bool lookingRight;
    [SerializeField] private LayerMask whatIsGround;
    private Rigidbody2D rb2D;
    private bool isDeath = false;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isDeath)
        {
            return;
        }
        RaycastHit2D groundInformation = Physics2D.Raycast(groundController.position, Vector2.down, distance, whatIsGround);

        rb2D.velocity = new Vector2(velocity, rb2D.velocity.y);

        if (groundInformation == false)
        {
            Flip();
        }
    }

    private void Flip()
    {
        lookingRight = !lookingRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180 , 0);
        velocity *= -1;
    }

    private void OnDrawGizmos()
    { 
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundController.transform.position, groundController.transform.position +Vector3.down * distance);
    }

    public void IsDeath()
    {
        isDeath = true;
        rb2D.gravityScale = 1;
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0; 
        rb2D.constraints = RigidbodyConstraints2D.None;  
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
