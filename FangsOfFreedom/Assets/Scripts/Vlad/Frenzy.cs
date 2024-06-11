using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frenzy : MonoBehaviour
{
    private Stats stats;
    private Combos combos;

    private void Start()
    {
        combos = GetComponent<Combos>();
        stats = GetComponent<Stats>();
    }

    public void IsInFrenzy()
    {
        combos.stopFrenzy = true;
        stats.movementSpeed *= 1.5f;
        stats.jumpForce *= 1.25f;
        stats.ChangeGravity(3);
    }

    public void FinishFrenzy()
    {
        combos.stopFrenzy = false;
        stats.movementSpeed /= 1.5f;
        stats.jumpForce /= 1.25f;
        stats.ChangeGravity(2.1f);
    }
}
