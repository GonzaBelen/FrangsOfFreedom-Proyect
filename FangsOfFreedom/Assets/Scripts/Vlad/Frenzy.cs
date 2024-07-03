using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using static EventManager;
using static StaticsVariables;

public class Frenzy : MonoBehaviour
{
    [SerializeField] private GameObject frenzyMeter;
    private FrenzyBar frenzyBar;
    private Stats stats;
    private Combos combos;
    [SerializeField] private float comboValue;
    private int timesFrenzy;
    private Timer timer;
    [SerializeField] GameObject timerObject;
    [SerializeField] GameObject[] eyes;

    private void Start()
    {
        timer = timerObject.gameObject.GetComponent<Timer>();
        frenzyBar = frenzyMeter.GetComponent<FrenzyBar>();
        combos = GetComponent<Combos>();
        stats = GetComponent<Stats>();
        timesFrenzy = 0;
    }

    private void FixedUpdate()
    {
        if (!combos.isInFrenzy && frenzyBar.currentFrenzy >= 99.5f)
        {
            combos.isInFrenzy = true;
            frenzyBar.frenzyReduction *= 2;
        } else if (combos.isInFrenzy && frenzyBar.currentFrenzy <= 0)
        {
            combos.isInFrenzy = false;
            frenzyBar.frenzyReduction /= 2;
        }
    }

    public void IsInFrenzy()
    {
        combos.stopFrenzy = true;
        stats.movementSpeed *= 1.5f;
        stats.jumpForce *= 1.25f;
        stats.ChangeGravity(5);
        SessionData.frenzyCounting++;
        SessionData.hasFrenzy = true;
        foreach (GameObject eye in eyes)
        {
            eye.SetActive(true);
        }
    }

    public void FinishFrenzy()
    {
        combos.stopFrenzy = false;
        stats.movementSpeed /= 1.5f;
        stats.jumpForce /= 1.25f;
        stats.ChangeGravity(3.5f);
        PowerEvent power = new PowerEvent
        {
            timeFrenzy = timer.frenzyTime,
        };

        AnalyticsService.Instance.RecordEvent(power);
        AnalyticsService.Instance.Flush();
        
        SessionData.hasFrenzy = false;
        timer.frenzyTime = 0;
        foreach (GameObject eye in eyes)
        {
            eye.SetActive(false);
        }
    }

    public void GainFrenzy()
    {
        frenzyBar.UpdateSlider(comboValue);
    }
}
