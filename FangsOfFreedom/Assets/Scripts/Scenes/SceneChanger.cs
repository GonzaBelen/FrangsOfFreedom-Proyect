using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static StaticsVariables;

public class SceneChanger : MonoBehaviour
{
    public GameObject sliderObject;
    public GameObject [] keyBoardImages;
    public GameObject controllerImage;
    private bool controller = false;
    private bool keyboard = false;

    private void Start()
    {
        SessionData.hasFrenzy = false;
        if (SessionData.controllerConnected)
        {
            controllerImage.SetActive(true);
        } else
        {
            int arraySelection = Random.Range(0,2);
            keyBoardImages[arraySelection].SetActive(true);
            Debug.Log(arraySelection);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J))
        {
            sliderObject.SetActive(true);
            StartCoroutine(AsynchronousLoad(SessionData.level));
        }

        if (controller && !SessionData.controllerConnected)
        {
            ChangeSignsFunction();
        } else if (keyboard && SessionData.controllerConnected)
        {
            ChangeSignsFunction();
        }

        if (SessionData.controllerConnected)
        {
            keyboard = false;
            controller = true;
        } else
        {
            keyboard = true;
            controller = false;
        }
    }

    IEnumerator AsynchronousLoad (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        // operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Slider slider = sliderObject.GetComponent<Slider>();
            slider.value = progress;

            yield return null;
        }
    }

    private void ChangeSignsFunction()
    {
        if (!SessionData.controllerConnected)
        {
            int arraySelection = Random.Range(0,2);
            keyBoardImages[arraySelection].SetActive(true);
            Debug.Log(arraySelection);
            controllerImage.SetActive(false);
        }

        if (SessionData.controllerConnected)
        {
            foreach (GameObject image in keyBoardImages)
            {
                image.SetActive(false);
            }
            controllerImage.SetActive(true);
        }
    }
}
