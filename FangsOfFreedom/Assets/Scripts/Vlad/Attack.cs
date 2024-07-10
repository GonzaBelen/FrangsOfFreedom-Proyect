using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEditor;
using static StaticsVariables;

public class Attack : MonoBehaviour
{
    private RespawnController respawnController;
    private Stats stats;
    private Combos combos;
    private Rigidbody2D rb2D;
    [SerializeField] private GameObject attackRange;
    [SerializeField] private LayerMask enemiesLayer;
    private AnimationController animationController;
    private PlayerController playerController;
    // public bool isAttacking = false;
    public bool canAttack = true;
    public float range;

    private void Start()
    {
        combos = GetComponent<Combos>();
        stats = GetComponent<Stats>();
        animationController = GetComponent<AnimationController>();
        playerController = GetComponent<PlayerController>();
        rb2D = GetComponent<Rigidbody2D>();
        respawnController = GetComponent<RespawnController>();
    }

    private void FixedUpdate()
    {
        if (playerController.isDashing)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackRange.transform.position, stats.attackRange, enemiesLayer);
            foreach (Collider2D enemie in enemies)
            {
                EnemiesController enemiesController = enemie.GetComponentInChildren<EnemiesController>();
                enemiesController.TakeDamage();
            }
        }
    }

    public void AttackAction (InputAction.CallbackContext context)
    {
        if (context.performed && canAttack && !playerController.stop)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.angularVelocity = 0;
            
            if (!SessionData.hasFrenzy)
            {
                animationController.ChangeAnimation("Attack");
            } else
            {
                animationController.ChangeAnimation("Attack-Frenzy");
            }
            
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackRange.transform.position, stats.attackRange, enemiesLayer);
            foreach (Collider2D enemie in enemies)
            {
                EnemiesController enemiesController = enemie.GetComponentInChildren<EnemiesController>();
                enemiesController.TakeDamage();
                if (!respawnController.hasTakeDamage)
                {
                    respawnController.multipleKills++;
                }                
                combos.Combo();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackRange.transform.position, range);
    }
}
