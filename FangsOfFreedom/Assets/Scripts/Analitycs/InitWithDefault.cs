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

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadTutorial()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("UI"), LayerMask.NameToLayer("Checkpoint"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("UI"), LayerMask.NameToLayer("Objects"), true);
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

    public void LoadLevel2()
    {
        SessionData.level = 2;
        LevelStartEvent levelStart = new LevelStartEvent
        {
            level = SessionData.level,
        };

        AnalyticsService.Instance.RecordEvent(levelStart);
        AnalyticsService.Instance.Flush();
        SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        SessionData.level = 3;
        LevelStartEvent levelStart = new LevelStartEvent
        {
            level = SessionData.level,
        };

        AnalyticsService.Instance.RecordEvent(levelStart);
        AnalyticsService.Instance.Flush();
        SceneManager.LoadScene("Level3");
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
        } else if (SessionData.level == 1)
        {
            LoadLevel1();
        } else if (SessionData.level == 2)
        {
            LoadLevel2();
        } else
        {
            LoadLevel3();
        }
    }
}
