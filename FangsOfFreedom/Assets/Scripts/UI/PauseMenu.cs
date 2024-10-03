using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticsVariables;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioSource audioSrc;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (SessionData.isPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("resume");
        audioSrc.UnPause();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SessionData.isPaused = false;
    }

    private void Pause()
    {
        Debug.Log("pause");
        audioSrc.Pause();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        SessionData.isPaused = true;
    }

    public void ChangeScene()
    {
        Debug.Log("cambio de escena");
        StopAllCoroutines();
        Time.timeScale = 1f;
    }
}