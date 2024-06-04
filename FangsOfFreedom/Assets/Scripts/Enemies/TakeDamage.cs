using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.Events;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private GameObject respwan;
    [SerializeField] private GameObject player;
    private Respawns respawns;
    private Rigidbody2D rb2D;
    private float gravity;
    [SerializeField] private float strength;
    public UnityEvent OnBeginRespawn;
    public UnityEvent OnDoneRespawn;

    private void Start()
    {
        rb2D = player.GetComponent<Rigidbody2D>();
        gravity = rb2D.gravityScale;
        respawns = respwan.GetComponent<Respawns>();
    }
    public void BeginRespawn()
    {
        OnBeginRespawn?.Invoke();
        rb2D.gravityScale = 0;
        rb2D.AddForce(transform.up * strength, ForceMode2D.Impulse);
        respawns.isTakingDamage = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), true);
    }

    public void DoneRespawn()
    {
        OnDoneRespawn?.Invoke();
        rb2D.gravityScale = 1.7f;
        respawns.isTakingDamage = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), false);
    }
}