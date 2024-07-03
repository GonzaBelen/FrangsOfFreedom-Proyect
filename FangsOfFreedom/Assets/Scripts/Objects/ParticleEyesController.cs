using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEyesController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] eyes;

    private void Start()
    {
        foreach (ParticleSystem particles in eyes)
        {
            particles.Stop();
        }
    }

    public void TurnOnParticles()
    {
        foreach (ParticleSystem particles in eyes)
        {
            particles.Play();
        }
    }

    public void TurnOffParticles()
    {
        foreach (ParticleSystem particles in eyes)
        {
            particles.Stop();
        }
    }
}
