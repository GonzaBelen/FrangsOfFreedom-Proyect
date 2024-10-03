using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    private BossController bossController;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootInterval = 2f;
    public bool canShoot;
    private void Start()
    {
        bossController = GetComponent<BossController>();
    }

    private IEnumerator ShootArrowsLoop()
    {
        while (canShoot)
        {
            Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
            yield return new WaitForSeconds(shootInterval);
        }
    }

    public void ShootManyArrows()
    {
        StartCoroutine(ShootArrowsLoop());
    }

    public void ShootArrow()
    {
        Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
        bossController.counter ++;
    }
}