using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
            gameObject.SetActive(false); // Desactivar el objeto cuando el sonido deja de reproducirse
            Invoke("ActiveBlood", 5f); // Activar el objeto después de 5 segundos
        }
    }

    private void ActiveBlood()
    {
        gameObject.SetActive(true); // Activar el objeto después de 5 segundos
    }
}