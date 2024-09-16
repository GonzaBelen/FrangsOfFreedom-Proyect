using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MovingPlatforms : MonoBehaviour
{
    [Header("Way Points")]
    [SerializeField] GameObject ways;
    [SerializeField] private Transform[] wayPoints;
    private int pointIndex;
    private int pointCount;
    private int direction = 1;
    [SerializeField] private int waitDuration;

    [Header("Player")]
    private PlayerController playerController;
    private Rigidbody2D rb2DPlayer;

    [Header("Platform")]
    [SerializeField] private float speed;
    private Vector3 target;
    
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Vector3 moveDirection;
    

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb2DPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        wayPoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.gameObject.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }

    private void Start()
    {
        pointIndex = 0;
        pointCount = wayPoints.Length;
        target = wayPoints[0].transform.position;
        DirectionCalculate();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, target) < 0.05f)
        {
            NextPoint();
        }
    }

    private void FixedUpdate()
    {
        rb2D.velocity = moveDirection * speed;
    }

    private void NextPoint()
    {
        transform.position = target;
        moveDirection = Vector3.zero;

        if (pointIndex == pointCount - 1)
        {
            direction = -1;
        }

        if (pointIndex == 0)
        {
            direction = 1;
        }

        pointIndex += direction;
        target = wayPoints[pointIndex].transform.position;
        WaitNextPoint();
    }

    private void DirectionCalculate()
    {
        moveDirection = (target - transform.position).normalized;
    }

    private async void WaitNextPoint()
    {
        await Task.Delay(waitDuration);
        DirectionCalculate();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.isOnPlatform = true;
            playerController.rb2DPlatform = rb2D;
            rb2DPlayer.gravityScale *= 50;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.isOnPlatform = false;
            if (!playerController.isJumping)
            {
                rb2DPlayer.gravityScale /= 50;
            }   
        }
    }
}
