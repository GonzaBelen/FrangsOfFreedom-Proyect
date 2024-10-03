using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EventManager;
using static StaticsVariables;

public class NewBehaviourScript : MonoBehaviour
{
    private bool firstSession = true;
    public GameObject vlad;
    async void Start()
    {
        if (firstSession)
        {
            await UnityServices.InitializeAsync();
            firstSession = false;
            ConsentGiven();
        }
    }

    public void ConsentGiven()
	{
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("UI"), LayerMask.NameToLayer("Checkpoint"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("UI"), LayerMask.NameToLayer("Objects"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("UI"), LayerMask.NameToLayer("Obstacles"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("UI"), LayerMask.NameToLayer("Curtain"), true);
        // SessionData.doubleJumpUnlock = false;
        // SessionData.frenzyUnlock = false;
        // SessionData.dashUnlock = false;
		AnalyticsService.Instance.StartDataCollection();
	}

    async public void LoadMenu()
    {
        SessionData.hasFrenzy = false;
        if (vlad != null && SessionData.isRespanwning)
        {
            RespawnController respawnController = vlad?.gameObject.GetComponent<RespawnController>();
            respawnController?.DoneRespawn();
        }

        while (SessionData.isRespanwning)
        {
            await Task.Yield();
        }

        if (!SessionData.isRespanwning)
        {
            SceneManager.LoadScene("MainMenu");
            SessionData.level = 1;
        }
    }

    // public void LoadTutorial()
    // {
    //     Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("UI"), LayerMask.NameToLayer("Checkpoint"), true);
    //     Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("UI"), LayerMask.NameToLayer("Objects"), true);
    //     SessionData.level = 0;
    //     LevelStartEvent levelStart = new LevelStartEvent
    //     {
    //         level = SessionData.level,
    //     };

    //     AnalyticsService.Instance.RecordEvent(levelStart);
    //     AnalyticsService.Instance.Flush();
    //     SceneManager.LoadScene("Tutorial");
    // }

    public void ResetDeaths()
    {
        SessionData.deathsCounting = 0;
    }

    async public void ChangeScene()
    {
        if (SessionData.isRespanwning)
        {
            RespawnController respawnController = vlad?.gameObject.GetComponent<RespawnController>();
            respawnController?.DoneRespawn();
        }

        while (SessionData.isRespanwning)
        {
            await Task.Yield();
        }
        SceneManager.LoadScene("LoadScreen");
    }
}