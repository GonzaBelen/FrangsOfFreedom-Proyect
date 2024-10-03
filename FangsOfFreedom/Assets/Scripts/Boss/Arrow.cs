using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb2D;
    [SerializeField] private int damage;
    [SerializeField] private float force;
    private GameObject vlad;
    private GameObject arrowReceptor;
    private Vector2 direction;

    private void Start()
    {
        vlad = GameObject.FindGameObjectWithTag("Player");
        Transform receptor = vlad.transform.Find("Arrow Receptor");
        arrowReceptor = receptor.gameObject;
        rb2D = GetComponent<Rigidbody2D>();
        direction = (arrowReceptor.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rb2D.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HungerController hungerController;
            hungerController = vlad.GetComponent<HungerController>();
            hungerController.ReduceHunger(damage);
            Destroy(gameObject);
        } else if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
