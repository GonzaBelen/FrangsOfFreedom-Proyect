using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EventManager;
using static StaticsVariables;

public class GameWinLevel1 : MonoBehaviour
{
    [SerializeField] private GameObject vlad;
    private RespawnController respawnController;
    private Timer timer;
    [SerializeField] GameObject timerObject;
    private Combos combos;
    private Blood blood;

    private void Start()
    {
        timer = timerObject.gameObject.GetComponent<Timer>();
        respawnController = vlad.gameObject.GetComponent<RespawnController>();
        combos = vlad.gameObject.GetComponent<Combos>();
        blood = vlad.gameObject.GetComponent<Blood>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            {
                SessionData.canCount = false;
                LevelCompleteEvent levelComplete = new LevelCompleteEvent
                {
                    level = SessionData.level,
                    flusks = SessionData.fluskCounting,
                    combo = combos.combo,
                    deaths = SessionData.deathsCounting,
                    safe = respawnController.hasTakeDamage,
                    time = timer.elapsedTime,
                    frenzy = SessionData.frenzyCounting,
                };

                AnalyticsService.Instance.RecordEvent(levelComplete);
                AnalyticsService.Instance.Flush();
                SceneManager.LoadScene("GameWin");
            }
        }
    }
}