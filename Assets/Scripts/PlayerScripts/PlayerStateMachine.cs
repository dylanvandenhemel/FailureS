using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine instance;

    public PlayerMovement moveScript;

    private void OnEnable()
    {
        instance = this;
    }

    public void PlayerFreeWill()
    {
        moveScript.enabled = true;
    }

    public void PlayerLock()
    {
        moveScript.enabled = false;

    }
}
