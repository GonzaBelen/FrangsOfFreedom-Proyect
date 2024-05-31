using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private string currentAnimation;
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimation (string newAnimation)
    {
        if (currentAnimation == newAnimation)
        {
            return;
        }
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
