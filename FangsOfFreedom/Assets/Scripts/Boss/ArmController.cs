using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            if (player.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, transform.localScale.z);
            }
            else if (player.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, -1, transform.localScale.z);
            }
        }

        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
