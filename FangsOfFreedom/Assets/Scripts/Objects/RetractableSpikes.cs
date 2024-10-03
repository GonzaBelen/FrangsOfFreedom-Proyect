using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RetractableSpikes : MonoBehaviour
{
    private AnimationController animationController;
    [SerializeField] private int delayDeploy;
    [SerializeField] private int delayRetract;
    [SerializeField] private int delayBetween;
    private PolygonCollider2D polygonCollider2D;
    private bool isDeploy = true;
    public bool hasDelay = false;

    private void Start()
    {
        animationController = GetComponent<AnimationController>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        if (hasDelay)
        {
            Delay();
        } else
        {
            Deploy();
        }        
    }

    private async void Deploy()
    {
        isDeploy = true;
        animationController.ChangeAnimation("Activation");
        await Task.Delay(delayDeploy);
        Retract();
    }

    private async void Retract()
    {
        isDeploy = false;
        ManageCollider();
        animationController.ChangeAnimation("Desactivation");
        await Task.Delay(delayRetract);
        Deploy();
    }

    public void ManageCollider()
    {
        if (isDeploy)
        {
            polygonCollider2D.enabled = true;
        } else
        {
            polygonCollider2D.enabled = false;
        }
    }

    private async void Delay()
    {
        await Task.Delay(delayRetract - delayBetween);
        Deploy();
    }
}
