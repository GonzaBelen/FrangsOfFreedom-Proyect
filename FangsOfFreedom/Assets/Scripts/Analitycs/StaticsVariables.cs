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
        public static float dialogueTimer;
        public static bool hasFrenzy = false;
        public static int level;
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
