using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;
using static StaticsVariables;

public class Pylon : MonoBehaviour
{
    private ParticleEyesController particleEyesController;
    [SerializeField] private Light2D[] eyes;
    [SerializeField] private float lighting;
    private bool addLighting = true;
    private bool changeState = false;
    private bool isInFrenzy = false;

    private void Start()
    {
        particleEyesController = GetComponent<ParticleEyesController>();
        foreach (Light2D light in eyes)
        {
            light.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (isInFrenzy)
        {
            if (addLighting)
            {
                foreach (Light2D light in eyes)
                {
                    light.intensity += Time.deltaTime * lighting;
                    if (light.intensity >= 0.75)
                    {
                        addLighting = false;
                    }
                }
            }

            if (!addLighting)
            {
                foreach (Light2D light in eyes)
                {
                    light.intensity -= Time.deltaTime * lighting;
                    if (light.intensity <= 0.15)
                    {
                        addLighting = true;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (SessionData.hasFrenzy && !changeState)
        {
            BeginFrenzy();
            changeState = true;
        }

        if (!SessionData.hasFrenzy && changeState)
        {
            DoneFrenzy();
            changeState = false;
        }
    }

    public void BeginFrenzy()
    {
        particleEyesController = GetComponent<ParticleEyesController>();
        foreach (Light2D light in eyes)
        {
            light.enabled = true;
        }
        isInFrenzy = true;
        particleEyesController.TurnOnParticles();
    }

    public void DoneFrenzy()
    {
        particleEyesController = GetComponent<ParticleEyesController>();
        foreach (Light2D light in eyes)
        {
            light.enabled = false;
        }
        isInFrenzy = false;
        particleEyesController.TurnOffParticles();
    }
}