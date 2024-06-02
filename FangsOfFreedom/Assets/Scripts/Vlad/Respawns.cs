using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawns : MonoBehaviour
{
    private TakeDamage takeDamage;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemie;
    [SerializeField] private float t;
    [SerializeField] private float speed;
    public bool isTakingDamage = false; 

    private void Start()
    {
        takeDamage = enemie.gameObject.GetComponent<TakeDamage>();
    }

    private void FixedUpdate()
    {
        if (isTakingDamage)
        {
            Vector3 a = player.transform.position;
            Vector3 b = target.position;
            player.transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            takeDamage.DoneRespawn();
        }
    }
}
