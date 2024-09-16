using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    [SerializeField] private int fallDelay;
    [SerializeField] private int resetDelay;
    [SerializeField] private Rigidbody2D rb2D;
    // private Vector2 initialPosition;
    [SerializeField] private AnimationController animationController;
    // [SerializeField] private GameObject prefab;
    [SerializeField] private BoxCollider2D boxCollider2D;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("UI"), LayerMask.NameToLayer("Ground"), true);
        // initialPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Fall();
        }
    }

    private async void Fall()
    {
        await Task.Delay(fallDelay);
        animationController.ChangeAnimation("Destroy");
        boxCollider2D.enabled = false;
        await ResetPosition();
        gameObject.SetActive(true);
        animationController.ChangeAnimation("Respawn");
    }

    private async Task ResetPosition()
    {
        await Task.Delay(resetDelay);
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    public void ActiveCollision()
    {
        boxCollider2D.enabled = true;
    }
}
