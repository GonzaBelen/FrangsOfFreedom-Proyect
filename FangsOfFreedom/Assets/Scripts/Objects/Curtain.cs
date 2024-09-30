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
        GameObject vlad = GameObject.Find("Vlad");
        LightExposure lightExposure = vlad.GetComponent<LightExposure>();
        lightExposure.ExitLightExposure();
        OnCloseCurtain?.Invoke();
        animationController.ChangeAnimation("Close");
    }
}
