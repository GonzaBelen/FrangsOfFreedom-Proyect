using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticsVariables;

public class CurtainController : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Light") && !SessionData.changeState)
        {
            SessionData.changeState = true;
            HungerController hungerController = GetComponentInParent<HungerController>();
            hungerController.FinishLightExposing();
        }
    }
}
