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
    public bool isAttacking = false;
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
                combos.Combo();
            }
        }
    }

    public void AttackAction ()
    {
        if (canAttack && !playerController.stop)
        {            
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
                if (enemie.CompareTag("Enemie"))
                {
                    EnemiesController enemiesController = enemie.GetComponentInChildren<EnemiesController>();
                    enemiesController.TakeDamage();
                    if (!respawnController.hasTakeDamage)
                    {
                        respawnController.multipleKills++;
                    }                
                    combos.Combo();
                }

                else if (enemie.CompareTag("Arrow"))
                {
                    Destroy(enemie.gameObject);
                    combos.Combo();
                } else if (enemie.CompareTag("Boss"))
                {
                    BossController bossController;
                    bossController = enemie.GetComponent<BossController>();
                    bossController.Damaged();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackRange.transform.position, range);
    }

    public void InAttack()
    {
        isAttacking = true;
    }

    public void FinishedAttack()
    {
        isAttacking = false;
    }
}
