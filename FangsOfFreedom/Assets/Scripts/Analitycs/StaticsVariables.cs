using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticsVariables : MonoBehaviour
{
    public static class SessionData
    {
        public static int deathsCounting;
        public static bool canCount;
        public static int fluskCounting;
        public static int frenzyCounting;
        public static bool hasFrenzy = false;
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
