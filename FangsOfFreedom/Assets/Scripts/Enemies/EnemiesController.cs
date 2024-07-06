using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesController : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    private AnimationController animationController;
    [SerializeField] private GameObject bat;
    public UnityEvent DeathEvent;
    //[SerializeField] private AudioSource clip;

    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        animationController = GetComponentInParent<AnimationController>();
    }

    public void TakeDamage()
    {
        animationController.ChangeAnimation("Death");
        circleCollider2D.isTrigger = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Death"), true);
        DeathEvent?.Invoke();
    }
}
