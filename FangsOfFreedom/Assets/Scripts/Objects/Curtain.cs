using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Curtain : MonoBehaviour
{
    private AnimationController animationController;
    public UnityEvent OnCloseCurtain;

    private void Start()
    {
        animationController = GetComponent<AnimationController>();
    }

    public void CloseCurtain()
    {
        OnCloseCurtain?.Invoke();
        animationController.ChangeAnimation("Close");
    }
}
