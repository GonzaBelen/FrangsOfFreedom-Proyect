using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damaged"))
        {
            EnemiesController enemiesController = other.GetComponentInChildren<EnemiesController>();
            enemiesController.TakeDamage();
        }
    }
}