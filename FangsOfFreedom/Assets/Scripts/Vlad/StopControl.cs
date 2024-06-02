using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopControl : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void NeedToStop()
    {
        playerController.stop = true;
    }

    public void DisableStop()
    {
        playerController.stop = false;
    }
}