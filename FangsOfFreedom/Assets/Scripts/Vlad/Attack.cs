using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemiesLayer;
    private AnimationController animationController;
    private PlayerController playerController;
    // public bool isAttacking = false;
    public bool canAttack = true;

    private void Start()
    {
        animationController = GetComponent<AnimationController>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {

    }

    public void AttackAction (InputAction.CallbackContext context)
    {
        if (context.performed && canAttack && !playerController.stop)
        {
            animationController.ChangeAnimation("Attack");
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemiesLayer);
            foreach (Collider2D enemie in enemies)
            {
                enemie.SendMessage("TakeDamage"); // Llama al método RecibirAtaque en el enemigo
            }
        }
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
