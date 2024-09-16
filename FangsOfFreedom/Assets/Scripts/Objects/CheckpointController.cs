using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

public class CheckpointController : MonoBehaviour
{
    private Vector3 checkpointPosition;
    [SerializeField] private AudioSource clip;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private Light2D[] candles;
    private bool isActive = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointPosition = transform.position;
        }
    }

    public Vector3 GetCheckpointPosition()
    {
        if (!isActive)
        {
            animationController.ChangeAnimation("Checkpoint-Activation");
        }
        isActive = true;
        clip.Play();
        return checkpointPosition;
    }

    public void FinishedAnimation()
    {
        animationController.ChangeAnimation("Checkpoint-Activated");
    }

    public void LightCandle()
    {
        foreach (Light2D light in candles)
        {
            light.enabled = true;
        }
    }
}