using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static StaticsVariables;

public class LightExposure : MonoBehaviour
{
    private HungerController hungerController;
    [SerializeField] private float exposure;
    [SerializeField] private float exposureEnd;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private float maxFlashLight;
    [SerializeField] private float minFlashLight;
    private bool isInExposure;

    private void Start()
    {
        hungerController = GetComponent<HungerController>();
    }

    private void FixedUpdate()
    {
        if (isInExposure)
        {
            if (globalLight.intensity <= maxFlashLight)
            {
                globalLight.intensity += exposure * Time.deltaTime;
            }
        } else 
        {
            if (globalLight.intensity >= minFlashLight)
            {
                globalLight.intensity -= exposureEnd * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            SessionData.changeState = false;
            hungerController.LightExposing();
            isInExposure = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            SessionData.changeState = false;
            hungerController.FinishLightExposing();
            isInExposure = false;
        }
    }
}
