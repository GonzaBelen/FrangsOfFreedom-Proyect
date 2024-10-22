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
    [SerializeField] private bool isInExposure;
    [SerializeField] private GameObject lightTransmitter;
    [SerializeField] private GameObject rayCastReceptor;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask player;
    public bool stopLightning = false;
    private bool isInWindowLight = false;

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

        if (lightTransmitter != null && rayCastReceptor != null)
        {
            Vector2 direction = (rayCastReceptor.transform.position - lightTransmitter.transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(lightTransmitter.transform.position, direction, rayLength, player);

            Debug.DrawRay(lightTransmitter.transform.position, direction * rayLength, Color.red);

            if (hit.collider != null) 
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    hungerController.FinishLightExposing();
                    isInExposure = false;
                    SessionData.changeState = false;
                    
                    Debug.DrawRay(lightTransmitter.transform.position, direction * hit.distance, Color.red);
                }
                else
                {
                    SessionData.changeState = false;
                    hungerController.LightExposing();
                    isInExposure = true;
                }
            }
        } else
        {
            //ExitLightExposure();
        }
    }

    private void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            stopLightning = false;
            lightTransmitter = other.gameObject;
        }

        if (other.CompareTag("WindowLight"))
        {
            hungerController.LightExposing();
            isInExposure = true;
            isInWindowLight = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            if (lightTransmitter == other.gameObject)
            {
                return;
            } else if (lightTransmitter != other.gameObject)
            {
                lightTransmitter = other.gameObject;
            } else 
            {
                lightTransmitter = null;
            }
        } else
        {
            lightTransmitter = null;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            if (lightTransmitter == other.gameObject)
            {
                stopLightning = true;
                ExitLightExposure();
                lightTransmitter = null;
            }
        }

         if (other.CompareTag("WindowLight"))
        {
            hungerController.FinishLightExposing();
            isInExposure = false;
            isInWindowLight = false;
        }
    }

    public void ExitLightExposure()
    {
        stopLightning = true;
        lightTransmitter = null;
        SessionData.changeState = false;
        hungerController.FinishLightExposing();
        isInExposure = false;
    }
}