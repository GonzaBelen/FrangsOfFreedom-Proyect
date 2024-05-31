using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float strength = 10f;
    [SerializeField] private AudioSource clip;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ApplyImpulse(other.gameObject.GetComponent<Rigidbody2D>());
            animator.Play("Trampoline");
            clip.Play();
        }
    }

    private void ApplyImpulse(Rigidbody2D rb)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * strength, ForceMode2D.Impulse);
    }

    public void ResetAnimation()
    {
        animator.Play("Trampoline-Initiator");
    }
}
