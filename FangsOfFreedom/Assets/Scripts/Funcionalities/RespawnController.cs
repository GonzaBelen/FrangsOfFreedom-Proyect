using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Events;
using static EventManager;
using static StaticsVariables;

public class RespawnController : MonoBehaviour
{
    private Combos combos;
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
    public int multipleKills;
    public bool hasTakeDamage = false;

    void Start()
    {
        combos = GetComponent<Combos>();
        rb2D = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
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
        positionToMove = lastCheckpoint.GetCheckpointPosition();
        Debug.Log("hay checkpoint");   
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
        FixGround();
    }

    public void DoneRespawn()
    {
        OnDoneRespawn?.Invoke();
        if (!combos.isInFrenzy)
        {
            rb2D.gravityScale = 3.5f;
        } else
        {
            rb2D.gravityScale = 5;
        }
        
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            if (!isTakingDamage)
            {
                lastCheckpoint = other.GetComponent<CheckpointController>();
                Debug.Log("Checkpoint alcanzado!");
                Respawn();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
         if (other.CompareTag("Checkpoint"))
        {
            if (isTakingDamage)
            {
                DoneRespawn();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemie") || other.transform.CompareTag("Obstacle"))
        {
            DamagedEvent damaged = new DamagedEvent
            {
                enemy = other.gameObject.name.ToString(),
                safeSlain = multipleKills,
                level = SessionData.level,
            };

            AnalyticsService.Instance.RecordEvent(damaged);
            AnalyticsService.Instance.Flush();
            hasTakeDamage = true;
            BeginRespawn();
        }
    }

    async void FixGround()
    {
        while (!stop)
        {
            rb2D.AddForce(direction * strength, ForceMode2D.Impulse);
            await Task.Delay(1000);
        }        
    }
}
