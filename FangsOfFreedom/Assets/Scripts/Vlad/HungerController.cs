using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static EventManager;
using Unity.Services.Analytics;

public class HungerController : MonoBehaviour
{
    private bool stopReceivingData = false;
    private Combos combos;
    [SerializeField] private HungerBar hungerBar;
    private float hunger = 100;
    private float hungerReduction = 10;
    [SerializeField] private float timeToReduceHunger = 3;
    private float currentTimeToReduceHunger;
    

    private void Start()
    {
        combos = GetComponent<Combos>();
        currentTimeToReduceHunger = timeToReduceHunger;
        hungerBar.UpdateSlider(hunger);
    }

    private void FixedUpdate()
    {
        currentTimeToReduceHunger -= Time.deltaTime;
        if(currentTimeToReduceHunger <= 0)
        {
            hunger -= hungerReduction;
            currentTimeToReduceHunger = timeToReduceHunger;
        }

        if (hunger < 0)
        {
            hunger = 0;
        }

        hungerBar.UpdateSlider(hunger);


        if (hunger <= 0)
        {
            if(!stopReceivingData)
            {
                stopReceivingData = true;
                GameOverEvent gameOver = new GameOverEvent
                {
                    level = 0,
                    deathsGO = 0,
                };

                AnalyticsService.Instance.RecordEvent(gameOver);
                AnalyticsService.Instance.Flush();
            }
            SceneManager.LoadScene("GameOver");
        }
    }

    public void GainHunger(int amount)
    {
        hunger += amount;
        hunger = Mathf.Clamp(hunger, 0, 100);
        combos.Combo();
    }

    public void LightExposing()
    {
        timeToReduceHunger = 2.5f;
    }

    public void FinishLightExposing()
    {
        timeToReduceHunger = 5;
    }
}
