using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activation : MonoBehaviour
{
    public UnityEvent OnCollide;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollide?.Invoke();
        }
    }
    
}
