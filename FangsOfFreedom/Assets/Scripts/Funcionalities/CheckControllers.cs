using UnityEngine;
using UnityEngine.InputSystem;
using static StaticsVariables;

public class CheckControllers : MonoBehaviour
{
    public bool isControllerConnected = false;

    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        CheckInitialConnectedDevices();
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void CheckInitialConnectedDevices()
    {
        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad)
            {
                isControllerConnected = true;
                SessionData.controllerConnected = true;
                Debug.Log("Controller already connected at startup: " + device.name);
                break;
            }
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            if (change == InputDeviceChange.Added)
            {
                isControllerConnected = true;
                SessionData.controllerConnected = true;
                Debug.Log("Controller connected: " + device.name);
            }
            else if (change == InputDeviceChange.Removed && device is Gamepad)
            {
                isControllerConnected = false;
                SessionData.controllerConnected = false;
                Debug.Log("Controller disconnected: " + device.name);
            }
        }
    }
}