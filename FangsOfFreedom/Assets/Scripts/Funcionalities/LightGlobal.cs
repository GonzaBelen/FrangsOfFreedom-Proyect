using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static StaticsVariables;

public class LightGlobal : MonoBehaviour
{
    private Light2D light2D;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color frenzyColor;
    private bool changeColor = true;

    private void Start()
    {
        light2D = GetComponent<Light2D>();
        light2D.color = baseColor;
    }

    private void Update()
    {
        if (SessionData.hasFrenzy && changeColor)
        {
            light2D.color = frenzyColor;
            changeColor = false;
        } else if (!SessionData.hasFrenzy && !changeColor)
        {
            light2D.color = baseColor;
            changeColor = true;
        }
    }
}
