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
    [SerializeField] private ActionController actionController;

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
    [SerializeField] private int remainingJumps;
    [SerializeField] public bool isJumping = false;
    [SerializeField] private bool jumpingUp;

    [Header("Movement")]
    [SerializeField] private float movementHor = 0;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float targetSpeed;

    [Header("Dash")]    
    public bool isDashing;
    [SerializeField] private bool canDash = true;
    [SerializeField] private int remainingDash = 1;

    [Header("Platforms")]
    public bool isOnPlatform;
    public Rigidbody2D rb2DPlatform;
    public float platformDirection;

    [Header("Secondary variables")]
    public bool stop = false;
    private bool changeAnimation = false;
    [SerializeField] private bool shouldCloseCurtain = false;
    public bool isInCurtain = false;
    public bool isInDialogue = false;
    public bool isInDialogueRange = false;
    private bool hasClosed = false;
    public bool changeLine = false;

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
        if (SessionData.doubleJumpUnlock)
        {
            remainingJumps = 2;
        } else 
        {
            remainingJumps = 1;
        }
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
            if (isDashing)
            {
                animationController.ChangeAnimation("Dash");
            }
            return;
        }

        isGrounded = Physics2D.OverlapBox(groundController.position, boxDimensions, 0f, whatIsGround);

        if (canMove)
        {
            targetSpeed = movementHor * stats.movementSpeed;
            rb2D.velocity = new Vector2(targetSpeed, rb2D.velocity.y);

            if (isOnPlatform)
            {
                rb2D.velocity = new Vector2(targetSpeed + rb2DPlatform.velocity.x, rb2D.velocity.y);
            }

            Flip(movementHor);
        }

        if (isJumping && rb2D.velocity.y <= 0)
        {
            jumpingUp = false;
            isJumping = false;
        } else if (rb2D.velocity.y >= 0)
            {
                jumpingUp = true;
            }

        if (isJumping && isGrounded)
        {
            isGrounded = false;
        }

        if (remainingJumps == 2)
        {
            if (!isJumping && !isGrounded)
            {
                remainingJumps = 1;
            }
        }

        if (!SessionData.doubleJumpUnlock)
        {
            if (!isJumping && !isGrounded)
            {
                remainingJumps = 0;
            }
        }

        if (isGrounded)
        {
            if (SessionData.doubleJumpUnlock)
            {
                remainingJumps = 2;
            } else
            {
                remainingJumps = 1;
            }
            remainingDash = 1;
            // attack.canAttack = true;
            if (movementHor != 0 && !attack.isAttacking)
            {
                if(!SessionData.hasFrenzy)
                {
                    animationController.ChangeAnimation("Move");
                } else
                {
                    animationController.ChangeAnimation("Move-Frenzy");
                }
            } else if (!attack.isAttacking)
            {
                if(!SessionData.hasFrenzy)
                {
                    animationController.ChangeAnimation("Idle");
                } else
                {
                    animationController.ChangeAnimation("Idle-Frenzy");
                }
            }
        } else if (!attack.isAttacking)
        {
            // attack.canAttack = false;
            if(!SessionData.hasFrenzy && jumpingUp)
            {
                animationController.ChangeAnimation("Jump");
            } else if (!jumpingUp)
            {
                animationController.ChangeAnimation("Jump-Down");
            }
            if (SessionData.hasFrenzy && jumpingUp)
            {
                animationController.ChangeAnimation("Jump-Frenzy");
            } else if (SessionData.hasFrenzy && !jumpingUp)
            {
                animationController.ChangeAnimation("Jump-Frenzy-Down");
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && remainingJumps > 0 && !stop && !isDashing)
        {
            if (isOnPlatform)
            {
                rb2D.gravityScale /= 50;
            }
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
        if (context.performed && canDash && combos.isInFrenzy && !stop && remainingDash > 0 && SessionData.dashUnlock)
        {
            StartCoroutine(Dash());
            remainingDash--;
        }
    }

    public void Action(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isInDialogueRange && actionController.isInCollideDialogue)
            {
                changeLine = true;
            } else if (isInCurtain && !actionController.isInCollide)
            {
                shouldCloseCurtain = true;
                hasClosed = true;
                StartCoroutine(StopCloseCurtain());
            } else 
            {
                attack.AttackAction();
            }
            
        }
    }

    public void BatJump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, stats.jumpForce);
        isJumping = true;
        if (SessionData.doubleJumpUnlock)
        {
            remainingJumps = 1;
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
            if(SessionData.doubleJumpUnlock)
            {
                remainingJumps = 1;
            } else
            {
                remainingJumps = 0;
            }  
            remainingDash = 1;
        }

        if (collider.gameObject.CompareTag("Curtain"))
        {
            isInCurtain = true;
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
            Curtain curtain = collider.gameObject.GetComponent<Curtain>();
            if (curtain != null && hasClosed)
            {
                curtain.CloseCurtain();
                combos.Combo();
                hasClosed = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Curtain"))
        {
            isInCurtain = false;
        }  
    }
}