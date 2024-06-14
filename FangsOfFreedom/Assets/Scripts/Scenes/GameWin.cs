using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EventManager;

public class GameWin : MonoBehaviour
{
    public float elapsedTime;

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            {
                LevelCompleteEvent levelComplete = new LevelCompleteEvent
                {
                    level = 0,
                    flusks = 0,
                    combo = 0,
                    deaths = 0,
                    safe = true,
                    time = 0,
                };

                AnalyticsService.Instance.RecordEvent(levelComplete);
                AnalyticsService.Instance.Flush();
                SceneManager.LoadScene("GameWin");
            }
        }
    }
}