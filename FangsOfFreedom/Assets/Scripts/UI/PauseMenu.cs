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

        if(SessionData.isPaused && Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            LoadMenu();
        } else if (SessionData.isPaused && Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Resume();
        }
    }

    public void Resume()
    {
        audioSrc.UnPause();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SessionData.isPaused = false;
    }

    private void Pause()
    {
        audioSrc.Pause();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        SessionData.isPaused = true;
    }

    public void LoadMenu()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
        //SceneManager.LoadScene("MainMenu");
    }

    public void UnPaused()
    {
        Time.timeScale = 1f;
    }
}