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
    private bool lightExposure = false;
    [SerializeField] private float hungerReductionMultiplier = 1;
    [SerializeField] private float timeCounter = 0;

    private void Start()
    {
        stats = GetComponent<Stats>();
        combos = GetComponent<Combos>();
        currentTimeToReduceHunger = stats.timeToReduceHunger;
        hungerBar.UpdateSlider(hunger);
    }

    private void FixedUpdate()
    {
        if (lightExposure)
        {
            timeCounter += Time.fixedDeltaTime;

            if (timeCounter >= 1)
                {
                    hungerReductionMultiplier *= 2;
                    timeCounter = 0;

                    Debug.Log("El multiplicador de hambre ha sido aumentado.");
                }
        }

        currentTimeToReduceHunger -= Time.deltaTime * hungerReductionMultiplier;
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


        if (hunger <= 0 && !SessionData.isRespanwning)
        {
            combos.isInFrenzy = false;
            Frenzy frenzy = GetComponent<Frenzy>();
            frenzy.FinishFrenzy();
            SessionData.hasFrenzy = false;
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
        hunger += amount;
        hunger = Mathf.Clamp(hunger, 0, 100);
        combos.Combo();
    }

    public void LightExposing()
    {
        lightExposure = true;
    }

    public void FinishLightExposing()
    {
        lightExposure = false;
        hungerReductionMultiplier = 1;
        timeCounter = 0;
    }
}
