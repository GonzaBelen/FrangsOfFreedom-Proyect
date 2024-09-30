using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RetractableSpikes : MonoBehaviour
{
    private AnimationController animationController;
    [SerializeField] private int timeToDeploy;
    [SerializeField] private int timeToRetract;
    private PolygonCollider2D polygonCollider2D;
    private bool isDeploy = true;

    private void Start()
    {
        animationController = GetComponent<AnimationController>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        Deploy();
    }

    private async void Deploy()
    {
        isDeploy = true;
        animationController.ChangeAnimation("Activation");
        await Task.Delay(timeToRetract);
        Retract();
    }

    private async void Retract()
    {
        isDeploy = false;
        ManageCollider();
        animationController.ChangeAnimation("Desactivation");
        await Task.Delay(timeToDeploy);
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
}
