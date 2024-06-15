using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticsVariables;

public class Timer : MonoBehaviour
{
    public float elapsedTime;
    public float frenzyTime;

    private void Start()
    {
        SessionData.canCount = true;
        SessionData.fluskCounting = 0;
        SessionData.frenzyCounting = 0;
    }

    private void FixedUpdate()
    {
        if (SessionData.canCount)
        {
            LevelTime();
        }

        if (SessionData.hasFrenzy)
        {
            FrenzyTime();
        }
    }

    public void LevelTime()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
    }

    public void FrenzyTime()
    {
        frenzyTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(frenzyTime / 60);
        int seconds = Mathf.FloorToInt(frenzyTime % 60);
    }
}
