using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopControl : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody2D rb2D;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void NeedToStop()
    {
        playerController.stop = true;
    }

    public void DisableStop()
    {
        playerController.stop = false;
    }
    public void Dialogue()
    {
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0;
        playerController.stop = true;
    }
}