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
        public static bool canChange = true;
        public static bool doubleJumpUnlock = false;
        public static bool frenzyUnlock = false;
        public static bool dashUnlock = false;
        public static bool controllerConnected = false;
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
