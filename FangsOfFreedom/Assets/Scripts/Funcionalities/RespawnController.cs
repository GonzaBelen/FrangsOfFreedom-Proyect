using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnController : MonoBehaviour
{
    [SerializeField] private LayerMask layersToIgnore;
    private Rigidbody2D rb2D;
    private Vector3 startPosition;
    private CheckpointController lastCheckpoint;
    [SerializeField] private float t;
    [SerializeField] private float speed;
    public bool isTakingDamage = false; 
    [SerializeField] private float strength;
    private Vector3 positionToMove;
    public UnityEvent OnBeginRespawn;
    public UnityEvent OnDoneRespawn;
    [SerializeField] private AudioSource clip;
    private Vector2 direction;
    private bool stop = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        Respawn();
    }

    private void FixedUpdate()
    {
        if (isTakingDamage)
        {
            Vector3 a = transform.position;
            Vector3 b = positionToMove;
            transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), speed);
        }
        direction = (positionToMove - transform.position).normalized;
    }

    private void Respawn()
    {
        if (lastCheckpoint != null)
        {
            positionToMove = lastCheckpoint.GetCheckpointPosition();
        }
        else
        {
            positionToMove = startPosition;
        }
    }

    public void BeginRespawn()
    {
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0; 
        OnBeginRespawn?.Invoke();
        rb2D.gravityScale = 0;
        rb2D.AddForce(transform.up * strength, ForceMode2D.Impulse);
        isTakingDamage = true;
        for (int i = 0; i < 32; i++)
        {
            if ((layersToIgnore.value & (1 << i)) != 0)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, true);
            }
        }
        stop = false;
        StartCoroutine(FixGround());
    }

    public void DoneRespawn()
    {
        OnDoneRespawn?.Invoke();
        rb2D.gravityScale = 2.1f;
        isTakingDamage = false;
        for (int i = 0; i < 32; i++)
        {
            if ((layersToIgnore.value & (1 << i)) != 0)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, false);
            }
        }
        stop = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            if (!isTakingDamage)
            {
                lastCheckpoint = other.GetComponent<CheckpointController>();
                Debug.Log("Checkpoint alcanzado!");
                Respawn(); // Llama al método para hacer respawn del jugador
                //clip.Play();
            } else 
            {
                DoneRespawn();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemie") || other.transform.CompareTag("Obstacle"))
        {
            BeginRespawn();
        }
    }

    private IEnumerator FixGround()
    {
        yield return new WaitForSeconds(2);
        if (!stop)
        {
            rb2D.AddForce(direction * strength, ForceMode2D.Impulse);
        }        
    }
}
