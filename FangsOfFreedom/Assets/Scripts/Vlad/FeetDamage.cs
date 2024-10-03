using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticsVariables;

public class FeetDamage : MonoBehaviour
{
    private PlayerController playerController;
    private Combos combos;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        combos = GetComponentInParent<Combos>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damaged"))
        {
            combos.Combo();
            playerController.BatJump();
            EnemiesController enemiesController = other.GetComponentInChildren<EnemiesController>();
            enemiesController.TakeDamage();
        }
    }
}