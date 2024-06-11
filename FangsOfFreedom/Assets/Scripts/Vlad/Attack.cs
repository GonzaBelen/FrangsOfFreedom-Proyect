using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    private Stats stats;
    [SerializeField] private GameObject attackRange;
    [SerializeField] private LayerMask enemiesLayer;
    private AnimationController animationController;
    private PlayerController playerController;
    // public bool isAttacking = false;
    public bool canAttack = true;
    public float range;

    private void Start()
    {
        stats = GetComponent<Stats>();
        animationController = GetComponent<AnimationController>();
        playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (playerController.isDashing)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackRange.transform.position, stats.attackRange, enemiesLayer);
            foreach (Collider2D enemie in enemies)
            {
                EnemiesController enemiesController = enemie.GetComponent<EnemiesController>();
                enemiesController.TakeDamage();
            }
        }
    }

    public void AttackAction (InputAction.CallbackContext context)
    {
        if (context.performed && canAttack && !playerController.stop)
        {
            animationController.ChangeAnimation("Attack");
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackRange.transform.position, stats.attackRange, enemiesLayer);
            foreach (Collider2D enemie in enemies)
            {
                EnemiesController enemiesController = enemie.GetComponent<EnemiesController>();
                enemiesController.TakeDamage();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackRange.transform.position, range);
    }

    // public void BeginAttack()
    // {
    //     isAttacking = true;
    // }

    // public void FinishedAttack()
    // {
    //     isAttacking = false;
    // }
}
