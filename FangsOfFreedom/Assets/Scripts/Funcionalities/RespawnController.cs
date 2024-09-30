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
    private string lastCheckpointName;
    private Frenzy frenzy;

    void Start()
    {
        frenzy = GetComponent<Frenzy>();
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
        // Debug.Log("Checkpoint");
        positionToMove = lastCheckpoint.GetCheckpointPosition();
        lastCheckpointName = lastCheckpoint.name;
    }

    public void BeginRespawn()
    {
        if (isTakingDamage)
        {
            return;
        }
        SessionData.canChange = false;
        if (SessionData.hasFrenzy)
        {
            frenzy.DisableEyes();
        }
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0; 
        OnBeginRespawn?.Invoke();
        rb2D.gravityScale = 0;
        rb2D.AddForce(transform.up * strength, ForceMode2D.Impulse);
        for (int i = 0; i < 32; i++)
        {
            if ((layersToIgnore.value & (1 << i)) != 0)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, true);
            }
        }
        stop = false;
        StartCoroutine(FixGround());
        isTakingDamage = true;
        SessionData.isRespanwning = true;
    }

    public void DoneRespawn()
    {
        if (!isTakingDamage)
        {
            return;
        }
        if (SessionData.hasFrenzy)
        {
            frenzy.ActiveEyes();
        }
        OnDoneRespawn?.Invoke();
        if (!combos.isInFrenzy)
        {
            rb2D.gravityScale = 3.5f;
        } else
        {
            rb2D.gravityScale = 5;
        }
        
        for (int i = 0; i < 32; i++)
        {
            if ((layersToIgnore.value & (1 << i)) != 0)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), i, false);
            }
        }
        stop = true;
        isTakingDamage = false;
        SessionData.canChange = true;
        SessionData.isRespanwning = false;
        Debug.Log("isRespawning termino y se puso en false");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            if (!isTakingDamage)
            {
                lastCheckpoint = other.GetComponent<CheckpointController>();
                if (!lastCheckpoint.isActive)
                {
                    Respawn();
                }
                // Debug.Log("Checkpoint alcanzado!");
                
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
         if (other.CompareTag("Checkpoint") && (other.name == lastCheckpointName))
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

    // async void FixGround()
    // {
    //     while (!stop)
    //     {
    //         rb2D.AddForce(direction * strength, ForceMode2D.Impulse);
    //         await Task.Delay(2000);
    //     }        
    // }

    private IEnumerator FixGround()
    {
        while (!stop)
        {
            rb2D.AddForce(direction * strength, ForceMode2D.Impulse);
            yield return new WaitForSeconds(2);
        }    
    }
}
