using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static StaticsVariables;

public class PlayerController : MonoBehaviour
{
    [Header("Scripts")]
    private Attack attack;
    private Stats stats;
    private Combos combos;
    private AnimationController animationController;
    private Dialogues dialogues;

    [Header("Components")]
    private Rigidbody2D rb2D;
    private PlayerInput playerInput;
    private Timer timer;
    [SerializeField] GameObject timerObject;
    
    [Header("Jump")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundController;
    [SerializeField] private Vector3 boxDimensions;
    [SerializeField] private bool isGrounded;
    [SerializeField] private int remainingJumps = 2;
    [SerializeField] private bool isJumping = false;

    [Header("Movement")]
    [SerializeField] private float movementHor = 0;
    [SerializeField] private bool canMove = true;

    [Header("Dash")]    
    public bool isDashing;
    [SerializeField] private bool canDash = true;
    [SerializeField] private int remainingDash = 1;

    [Header("Secondary variables")]
    public bool stop = false;
    private bool changeAnimation = false;
    [SerializeField] private bool shouldCloseCurtain = false;
    public bool isInDialogue = false;

     private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animationController = GetComponent<AnimationController>();
        attack = GetComponent<Attack>();
        stats = GetComponent<Stats>();
        combos= GetComponent<Combos>();
        playerInput = GetComponent<PlayerInput>();
        dialogues = GetComponent<Dialogues>();
        timer = timerObject.gameObject.GetComponent<Timer>();
    }

    private void FixedUpdate()
    {
        if (isInDialogue)
        {
            animationController.ChangeAnimation("Idle");
            timer.DialogueTime();
            SessionData.dialogueTimer = timer.dialogueTime;
            return;
        } else
        {
            timer.dialogueTime = 0;
            SessionData.dialogueTimer = timer.dialogueTime;
        }
        if(stop || isDashing)
        {
            return;
        }

        isGrounded = Physics2D.OverlapBox(groundController.position, boxDimensions, 0f, whatIsGround);

        if (canMove)
        {
            rb2D.velocity = new Vector2(movementHor * stats.movementSpeed, rb2D.velocity.y);
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
            remainingDash = 1;
            attack.canAttack = true;
            if (movementHor != 0)
            {
                if(!SessionData.hasFrenzy)
                {
                    animationController.ChangeAnimation("Move");
                } else
                {
                    animationController.ChangeAnimation("Move-Frenzy");
                }
            } else 
            {
                if(!SessionData.hasFrenzy)
                {
                    animationController.ChangeAnimation("Idle");
                } else
                {
                    animationController.ChangeAnimation("Idle-Frenzy");
                }
            }
        } else 
        {
            attack.canAttack = false;
            if(!SessionData.hasFrenzy)
            {
                animationController.ChangeAnimation("Jump");
            } else
            {
                animationController.ChangeAnimation("Jump-Frenzy");
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && remainingJumps > 0 && !stop && !isDashing)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, stats.jumpForce);
            isJumping = true;
            remainingJumps--;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementHor = context.ReadValue<Vector2>().x;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && combos.isInFrenzy && !stop && remainingDash > 0)
        {
            StartCoroutine(Dash());
            remainingDash--;
        }
    }

    public void Curtain(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            shouldCloseCurtain = true;
            StartCoroutine(StopCloseCurtain());
        }
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
            remainingDash = 1;
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

    public IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Obstacles"), true);
        isDashing = true;
        canDash = false;
        canMove = false;
        stats.ChangeGravity(0);
        rb2D.velocity = new Vector2(transform.localScale.x * stats.dashForce, 0f);
        yield return new WaitForSeconds(stats.dashingTime);
        stats.ChangeGravity(5f);
        canMove = true;
        isDashing = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Obstacles"), false);
        yield return new WaitForSeconds(stats.coolDown);
        canDash = true;
    }

    private IEnumerator StopCloseCurtain()
    {
        yield return new WaitForSeconds(1);
        shouldCloseCurtain = false;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (shouldCloseCurtain && collider.gameObject.CompareTag("Curtain"))
        {
            Debug.Log("Se debe cerrar cortina");
            Curtain curtain = collider.gameObject.GetComponent<Curtain>();
            if (curtain != null)
            {
                curtain.CloseCurtain();
                combos.Combo();
            }
        }
    }
}