using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static EventManager;
using static StaticsVariables;

public class ButtonPressJoystick : MonoBehaviour
{
    private bool isAlreadyChangeScene = false;
    public UnityEvent OnPressA;
    public UnityEvent OnPressX;
    public UnityEvent OnPressB;

    private void Awake()
    {
        isAlreadyChangeScene = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isAlreadyChangeScene)
        {
            OnPressA?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton2) && !isAlreadyChangeScene)
        {
            isAlreadyChangeScene = true;
            OnPressX?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton1) && !isAlreadyChangeScene)
        {
            isAlreadyChangeScene = true;
            OnPressB?.Invoke();
        }
    }
}
