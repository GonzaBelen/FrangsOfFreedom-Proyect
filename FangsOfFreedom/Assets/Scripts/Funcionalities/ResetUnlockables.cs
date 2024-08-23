using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticsVariables;

public class ResetUnlockables : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        SessionData.doubleJumpUnlock = false;
        SessionData.frenzyUnlock = false;
        SessionData.dashUnlock = false;
    }
}
