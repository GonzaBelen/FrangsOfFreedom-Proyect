using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class BossController : MonoBehaviour
{
    private ArrowShoot arrowShoot;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject stunPoint;
    [SerializeField] private int shootDelay;
    [SerializeField] private int spawnDelay;
    [SerializeField] private int arrowsToStun;
    public int counter;
    [SerializeField] private int life = 3;
    private bool isStun = false;
    private bool isChangedAlready = false;

    private void Start()
    {
        arrowShoot = GetComponent<ArrowShoot>();
        PositionChanger();
    }

    private void Update()
    {
        if (counter == arrowsToStun && !isChangedAlready)
        {
            isStun = true;
            isChangedAlready = true;
            Stunned();
        }

        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private async void PositionChanger()
    {
        gameObject.SetActive(true);
        SpawnController();
        await Task.Delay(shootDelay);
        if (isStun)
        return;
        arrowShoot.ShootArrow();
        gameObject.SetActive(false);
        SpawnDelay();
    }

    private async void SpawnDelay()
    {
        if (isStun)
        return;
        await Task.Delay(spawnDelay);
        PositionChanger();
    }

    private void SpawnController()
    {
        if (spawnPoints.Length > 0 && !isStun)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            transform.position = spawnPoints[randomIndex].transform.position;
        }
    }

    private void Stunned()
    {
        transform.position = stunPoint.transform.position;
        gameObject.SetActive(true);
        arrowShoot.canShoot = true;
        arrowShoot.ShootManyArrows();
    }

    public void Damaged()
    {
        Debug.Log("Se llamo al danio del jefe");
        if (isStun)
        {
            counter = 0;
            isChangedAlready = false;
            Debug.Log("Se danio al jefe");
            shootDelay /= 2;
            spawnDelay /= 2;
            life--;
            arrowsToStun *= 2;
            isStun = false;
            arrowShoot.canShoot = false;
            arrowShoot.StopAllCoroutines();
            PositionChanger();
        }
    }
}
