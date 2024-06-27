using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CheckpointController : MonoBehaviour
{
    private Vector3 checkpointPosition;
    [SerializeField] private AudioSource clip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointPosition = transform.position;
        }
    }

    public Vector3 GetCheckpointPosition()
    {
        clip.Play();
        return checkpointPosition;
    } 
}