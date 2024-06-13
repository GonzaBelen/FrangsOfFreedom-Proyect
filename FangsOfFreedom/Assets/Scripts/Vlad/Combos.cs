using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combos : MonoBehaviour
{
    private Frenzy frenzy;
    public bool isInFrenzy = false;
    public bool stopFrenzy = false;
    [SerializeField] private int combo = 0;

    private void Start()
    {
        frenzy = GetComponent<Frenzy>();
    }

    private void FixedUpdate()
    {
        if (isInFrenzy && !stopFrenzy)
        {
            frenzy.IsInFrenzy();
        } else if (!isInFrenzy && stopFrenzy)
        {
            frenzy.FinishFrenzy();
        }
    }

    public void Combo()
    {
        combo++;
        frenzy.GainFrenzy();
    }
}
