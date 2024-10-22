using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static StaticsVariables;

public class ChangeLevel : MonoBehaviour
{
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.N)) && SessionData.canChangeLevel)
        {
            SessionData.canChangeLevel = false;
            Debug.Log("se cambio de nivel con la N o la Y");
            SessionData.level++;
            SceneManager.LoadScene("LoadScreen");
        }
    }
}
