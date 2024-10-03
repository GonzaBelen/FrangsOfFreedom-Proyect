using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorController : MonoBehaviour
{
    public float rotationSpeed;
    [SerializeField] private bool rotateRight = true;
    private bool isOn = true;

    private void FixedUpdate()
    {
        if (!isOn)
        {
            return;
        }

        if (rotateRight)
        {
            transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
        } else
        {
            transform.Rotate(0, 0, -(Time.deltaTime * rotationSpeed));
        }
    }

    private void Update()
    {
        float zRotation = transform.eulerAngles.z;

        if (zRotation > 180)
        {
            zRotation -= 360;
        }

        if (zRotation < -20)
        {
            rotateRight = true;
        }
        else if (zRotation > 20)
        {
            rotateRight = false;
        }
    }
}
