using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static EventManager;
using Unity.Services.Analytics;
using static StaticsVariables;

public class HungerController : MonoBehaviour
{
    private Stats stats;
    private bool stopReceivingData = false;
    private Combos combos;
    [SerializeField] private HungerBar hungerBar;
    private float hunger = 100;
    private float currentTimeToReduceHunger;
    

    private void Start()
    {
        stats = GetComponent<Stats>();
        combos = GetComponent<Combos>();
        currentTimeToReduceHunger = stats.timeToReduceHunger;
        hungerBar.UpdateSlider(hunger);
    }

    private void FixedUpdate()
    {
        currentTimeToReduceHunger -= Time.deltaTime;
        if(currentTimeToReduceHunger <= 0)
        {
            hunger -= stats.hungerReduction;
            currentTimeToReduceHunger = stats.timeToReduceHunger;
        }

        if (hunger < 0)
        {
            hunger = 0;
        }

        hungerBar.UpdateSlider(hunger);


        if (hunger <= 0)
        {
            SessionData.deathsCounting++;
            SessionData.canCount = false;
            if(!stopReceivingData)
            {
                stopReceivingData = true;
                GameOverEvent gameOver = new GameOverEvent
                {
                    level = SessionData.level,
                    deathsGO = SessionData.deathsCounting,
                };

                AnalyticsService.Instance.RecordEvent(gameOver);
                AnalyticsService.Instance.Flush();
            }
            SceneManager.LoadScene("GameOver");
        }
    }

    public void GainHunger(int amount)
    {
        //parametro para saber si junto botella
        hunger += amount;
        hunger = Mathf.Clamp(hunger, 0, 100);
        combos.Combo();
    }

    public void LightExposing()
    {
        stats.timeToReduceHunger = 2.5f;
    }

    public void FinishLightExposing()
    {
        stats.timeToReduceHunger = 5;
    }
}
