using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static StaticsVariables;

public class Blood : MonoBehaviour
{
    [SerializeField] private AudioSource clip;
    private bool isPlaying = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HungerController hungerController = other.GetComponent<HungerController>();
            if (hungerController != null)
            {
                hungerController.GainHunger(10);
                SessionData.fluskCounting++;
                if (!isPlaying)
                {
                    clip.Play();
                    isPlaying = true;
                }
            }
        }
    }

    private void Update()
    {
        if (isPlaying && !clip.isPlaying)
        {
            isPlaying = false;
            gameObject.SetActive(false);
            Invoke("ActiveBlood", 5f);
        }
    }

    private void ActiveBlood()
    {
        gameObject.SetActive(true);
    }
}