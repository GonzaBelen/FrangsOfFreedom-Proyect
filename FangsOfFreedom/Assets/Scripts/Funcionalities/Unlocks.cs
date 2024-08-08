using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticsVariables;

public class Unlocks : MonoBehaviour
{
    public void DoubleJump()
    {
        SessionData.doubleJumpUnlock = true;
    }

    public void Frenzy()
    {
        SessionData.frenzyUnlock = true;
    }

    public void Dash()
    {
        SessionData.dashUnlock = true;
    }
}
