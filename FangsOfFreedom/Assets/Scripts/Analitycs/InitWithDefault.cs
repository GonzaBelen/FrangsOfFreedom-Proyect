using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EventManager;
using static StaticsVariables;

public class NewBehaviourScript : MonoBehaviour
{
    private bool firstSession = true;
    async void Start()
    {
        if (firstSession)
        {
            await UnityServices.InitializeAsync();
            firstSession = false;
        }
    }

    public void ConsentGiven()
	{
		AnalyticsService.Instance.StartDataCollection();
	}

    public void LoadTutorial()
    {
        SessionData.level = 0;
        LevelStartEvent levelStart = new LevelStartEvent
        {
            level = SessionData.level,
        };

        AnalyticsService.Instance.RecordEvent(levelStart);
        AnalyticsService.Instance.Flush();
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadLevel1()
    {
        SessionData.level = 1;
        LevelStartEvent levelStart = new LevelStartEvent
        {
            level = SessionData.level,
        };

        AnalyticsService.Instance.RecordEvent(levelStart);
        AnalyticsService.Instance.Flush();
        SceneManager.LoadScene("Level1");
    }

    public void ResetDeaths()
    {
        SessionData.deathsCounting = 0;
    }

    public void RestartScene()
    {
        if (SessionData.level == 0)
        {
            LoadTutorial();
        } else
        {
            LoadLevel1();
        }
    }
}
