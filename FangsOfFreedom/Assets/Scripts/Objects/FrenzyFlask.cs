using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticsVariables;

public class FrenzyFlask : MonoBehaviour
{
    [SerializeField] private GameObject frenzyMeter;
    private FrenzyBar frenzyBar;

    private void Start()
    {
        frenzyBar = frenzyMeter.GetComponent<FrenzyBar>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            frenzyBar.frenzyReduction = 0;
            Frenzy frenzy;
            frenzy = other.GetComponent<Frenzy>();
            Combos combos;
            combos = other.GetComponent<Combos>();
            frenzy.GainFrenzy();
            SessionData.hasFrenzy = true;
            frenzy.beginInFrenzy = true;
            frenzyBar.beginInFrenzy = true;
            combos.isInFrenzy = true;
            Destroy(gameObject);
        }
    }
}