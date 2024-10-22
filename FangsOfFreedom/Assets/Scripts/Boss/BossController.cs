using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class BossController : MonoBehaviour
{
    private AnimationController animationController;
    private ArrowShoot arrowShoot;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject stunPoint;
    [SerializeField] private int shootDelay;
    [SerializeField] private int spawnDelay;
    [SerializeField] private int arrowsToStun;
    public int counter;
    [SerializeField] private int life = 3;
    public bool isStun = false;
    private bool isChangedAlready = false;
    [SerializeField] private GameObject spawnPoint;
    public bool isInFinalBattle;
    public bool hasChanged = false;
    public UnityEvent EnableArm;
    [SerializeField] private GameObject arm;
    private Transform player;

    private void Start()
    {
        animationController = GetComponent<AnimationController>();
        arrowShoot = arm.GetComponent<ArrowShoot>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PositionChanger();
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1.5f, transform.localScale.y, transform.localScale.z);
            }
            else if (player.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1.5f, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isInFinalBattle)
            return;
        if (counter == arrowsToStun && !isChangedAlready)
        {
            isStun = true;
            isChangedAlready = true;
        }

        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void PositionChanger()
    {
        hasChanged = true;
        gameObject.SetActive(true);
        SpawnController();
        SmokeAnim(); 
    }

    private async void SpawnDelay()
    {
        if (isStun)
            return;
        await Task.Delay(spawnDelay);
        if (isInFinalBattle)
        {
            counter ++;
            if (counter != arrowsToStun)
            {
                PositionChanger();
            } else 
            {
                Stunned();
            }
        } else 
        {
            PositionChanger();
        }
    }

    private void SpawnController()
    {
        if (isInFinalBattle && !isStun)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            transform.position = spawnPoints[randomIndex].transform.position;
        } else if (!isInFinalBattle)
        {
            transform.position = spawnPoint.transform.position;
        }
    }

    public void Shoot()
    {
        Debug.Log("Se llamo a shoot");
         if (life == 2)
        {
            arrowShoot.needToShootAgain = true;
            arrowShoot.ShootArrowsWithInterval(2, 0.25f);
        }
        else if (life == 1)
        {
            arrowShoot.needToShootAgain = true;
            arrowShoot.ShootArrowsWithInterval(3, 0.25f);
        }
        else
        {
            Debug.Log("Se disparo una flecha");
            arrowShoot.ShootArrow();
            hasChanged = false;
        }
    }

    private void Stunned()
    {
        transform.position = stunPoint.transform.position;
        gameObject.SetActive(true);
        arrowShoot.canShoot = true;
        arrowShoot.needToShootAgain = true;
    }

    public void Damaged()
    {
        if (isStun)
        {
            hasChanged = false;
            counter = 0;
            isChangedAlready = false;
            spawnDelay /= 2;
            life--;
            arrowsToStun *= 2;
            isStun = false;
            arrowShoot.needToShootAgain = false;
            arrowShoot.canShoot = false;
            arrowShoot.StopAllCoroutines();
            SmokeAnim();
        }
    }

    public void AnimControl()
    {
        if (isStun)
        {
            animationController.ChangeAnimation("Shoot");
            return;
        }

        if (hasChanged)
        {
            animationController.ChangeAnimation("Shoot");
        } else
        {
            gameObject.SetActive(false);
            SpawnDelay();
        }
    }

    public void SmokeAnim()
    {
        arm.SetActive(false);
        animationController.ChangeAnimation("Smoke-Boss");
    }

    public void ChangeToSmoke()
    {
        Debug.Log("Se llama a change to smoke");
        if(!isStun && !arrowShoot.needToShootAgain)
        {
            Debug.Log(arrowShoot.needToShootAgain + "se hace la animacion de smoke");
            SmokeAnim();
        } else
        {
            Debug.Log(arrowShoot.needToShootAgain + "se hace el idle para que vuelba a disparar");
            arrowShoot.animationController.ChangeAnimation("Idle");
        }
    }

    public void Arm()
    {
        if (isStun)
        {
            arm.SetActive(true);
            arrowShoot.ShootManyArrows();
        }
        EnableArm?.Invoke();
    }
}