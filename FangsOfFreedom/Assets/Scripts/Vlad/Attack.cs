using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemiesLayer;
    private AnimationController animationController;

    private void Start()
    {
        animationController = GetComponent<AnimationController>();
    }

    private void Update()
    {

    }

    public void AttackAction (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animationController.ChangeAnimation("Attack");
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemiesLayer);
            foreach (Collider2D enemie in enemies)
            {
                enemie.SendMessage("TakeDamage"); // Llama al método RecibirAtaque en el enemigo
            }
        }
    }
}
