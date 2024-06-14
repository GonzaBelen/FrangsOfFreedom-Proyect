using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frenzy : MonoBehaviour
{
    [SerializeField] private GameObject frenzyMeter;
    private FrenzyBar frenzyBar;
    private Stats stats;
    private Combos combos;
    [SerializeField] private float comboValue;

    private void Start()
    {
        frenzyBar = frenzyMeter.GetComponent<FrenzyBar>();
        combos = GetComponent<Combos>();
        stats = GetComponent<Stats>();
    }

    private void FixedUpdate()
    {
        if (!combos.isInFrenzy && frenzyBar.currentFrenzy >= 100)
        {
            combos.isInFrenzy = true;
            frenzyBar.frenzyReduction *= 1.5f;
        } else if (combos.isInFrenzy && frenzyBar.currentFrenzy <= 0)
        {
            combos.isInFrenzy = false;
            frenzyBar.frenzyReduction /= 1.5f;
        }
    }

    public void IsInFrenzy()
    {
        combos.stopFrenzy = true;
        stats.movementSpeed *= 1.5f;
        stats.jumpForce *= 1.25f;
        stats.ChangeGravity(5);
    }

    public void FinishFrenzy()
    {
        combos.stopFrenzy = false;
        stats.movementSpeed /= 1.5f;
        stats.jumpForce /= 1.25f;
        stats.ChangeGravity(3.5f);
    }

    public void GainFrenzy()
    {
        frenzyBar.UpdateSlider(comboValue);
    }
}
