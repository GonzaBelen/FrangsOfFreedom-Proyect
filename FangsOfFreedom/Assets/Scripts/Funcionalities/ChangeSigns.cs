using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static StaticsVariables;

public class ChangeSigns : MonoBehaviour
{
    [SerializeField] private GameObject[] keyboardSigns;
    [SerializeField] private GameObject[] controllerSigns;
    private bool controller = false;
    private bool keyboard = false;
    public UnityEvent Unlock;


    private void Start()
    {
        ChangeSignsFunction();
        Unlock?.Invoke();
    }

    private void Update()
    {
        if (controller && !SessionData.controllerConnected)
        {
            ChangeSignsFunction();
        } else if (keyboard && SessionData.controllerConnected)
        {
            ChangeSignsFunction();
        }

        if (SessionData.controllerConnected)
        {
            keyboard = false;
            controller = true;
        } else
        {
            keyboard = true;
            controller = false;
        }
    }

    private void ChangeSignsFunction()
    {
        if (!SessionData.controllerConnected)
        {
            foreach (GameObject sign in keyboardSigns)
            {
                sign.SetActive(true);
            }
            foreach (GameObject sign in controllerSigns)
            {
                sign.SetActive(false);
            }
        }

        if (SessionData.controllerConnected)
        {
            foreach (GameObject sign in keyboardSigns)
            {
                sign.SetActive(false);
            }
            foreach (GameObject sign in controllerSigns)
            {
                sign.SetActive(true);
            }
        }
    }
}
