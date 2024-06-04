using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private AnimationController animationController;
    private Attack attack;
    public bool stop = false;
    private bool changeAnimation = false;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundController;
    [SerializeField] private Vector3 boxDimensions;
    [SerializeField] private bool isGrounded;
    [SerializeField] private int remainingJumps = 2;
    [SerializeField] private bool isJumping = false;

    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    private float movementHor = 0;
    private bool canMove = true;

     private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animationController = GetComponent<AnimationController>();
        attack = GetComponent<Attack>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(stop)
        {
            return;
        }

        isGrounded = Physics2D.OverlapBox(groundController.position, boxDimensions, 0f, whatIsGround);

        if (canMove)
        {
            rb2D.velocity = new Vector2(movementHor * movementSpeed, rb2D.velocity.y);
            Flip(movementHor);
        }

        if (isJumping && rb2D.velocity.y <= 0)
        {
            isJumping = false;
        }

        if (isJumping && isGrounded)
        {
            isGrounded = false;
        }

        if (isGrounded)
        {
            remainingJumps = 2;
            attack.canAttack = true;
            if (movementHor != 0)
            {
                animationController.ChangeAnimation("Move");
            } else 
            {
                animationController.ChangeAnimation("Idle");
            }
        } else 
        {
            attack.canAttack = false;
            animationController.ChangeAnimation("Jump");
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && remainingJumps > 0 && !stop)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            isJumping = true;
            remainingJumps--;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementHor = context.ReadValue<Vector2>().x;
    }

    private void Flip (float lookDirection)
    {
        Vector3 scale = transform.localScale;
        if (lookDirection > 0)
        {
            scale.x = 2.5f;
        }
        else if (lookDirection < 0)
        {
            scale.x = -2.5f;
        }
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundController.position, boxDimensions);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Trampoline"))
        {
            isJumping = true;
            remainingJumps = 1;
        }
    }

    public void SmokeAnimation()
    {
        changeAnimation = !changeAnimation;
        animationController.ChangeAnimation("Smoke");
    }

    public void BatAnimation()
    {
        if (changeAnimation)
        {
            animationController.ChangeAnimation("Bat");
        }
    }

    public void NowStop()
    {
        if (!changeAnimation)
        {
            stop = false;
        }
    }
}
