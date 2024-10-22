using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject spawnPoint;

    private void Update()
    {
        GenerateRaycast();
    }

    private void GenerateRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, Vector2.down, Mathf.Infinity, groundLayer);

        if (hit.collider != null)
        {
            Debug.DrawLine(rayOrigin.position, hit.point, Color.red);
            spawnPoint.transform.position = hit.point;
        }
    }
}