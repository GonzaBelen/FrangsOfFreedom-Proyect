using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrowShoot : MonoBehaviour
{
    public AnimationController animationController;
    private BossController bossController;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootInterval = 2f;
    public bool canShoot;
    public bool needToShootAgain = false;
    public UnityEvent Helper;
    private void Start()
    {
        animationController = GetComponent<AnimationController>();
        bossController = GetComponentInParent<BossController>();
    }

    private IEnumerator ShootArrowsLoop()
    {
        while (canShoot)
        {
            animationController.ChangeAnimation("Shoot-Again");
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
        Debug.Log("Se llamo a shootarrow");
        animationController.ChangeAnimation("Shoot-Again");
        Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
    }

    public void ShootArrowsWithInterval(int numberOfArrows, float interval)
    {
        StartCoroutine(ShootArrowsWithIntervalCoroutine(numberOfArrows, interval));
    }

    private IEnumerator ShootArrowsWithIntervalCoroutine(int numberOfArrows, float interval)
    {
        for (int i = 0; i < numberOfArrows; i++)
        {
            ShootArrow();
            if (i == (numberOfArrows - 1))
            {
                needToShootAgain = false;
                bossController.hasChanged = false;
            }
            yield return new WaitForSeconds(interval);
        }
    }

    public void Help()
    {
        Helper?.Invoke();
    }
}