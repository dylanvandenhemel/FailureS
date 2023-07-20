using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance;
    PlayerControls pActions;

    public GameObject miniGameGroup;

    [Header("Ring Game")]
    public GameObject ringMatchGame;
    public RectTransform ringToMatchObject;
    public RectTransform ringToScaleObject;
    private bool bRingGameActive;

    //all games info
    private int miniGameID = 1;
    private int difficultyScale;
    private bool bInSpace;
    private int totalWonGames;
    private int roundOfGames = 3;

    private void OnEnable()
    {
        instance = this;

        pActions = new PlayerControls();
        pActions.Enable();
    }
    private void OnDisable()
    {
        pActions.Disable();
        pActions.PlayerActions.Mouse1.started -= MiniGameClick;
    }

    public void StartMiniGame(int difficulty)
    {
        Debug.Log("start game");
        miniGameGroup.SetActive(true);
        difficultyScale = difficulty;
        StartCoroutine(miniGameStart(difficultyScale));
    }
    IEnumerator miniGameStart(int difficultyScale)
    {
        //will be rand later
        //miniGameID++;
        //

        yield return new WaitForSeconds(1);
        //ring match
        if(miniGameID == 1)
        {
            RingMatch(difficultyScale);
        }
        else if(miniGameID == 2)
        {

        }
        else if(miniGameID == 3)
        {

        }

    }

    //ring increases to size of match ring, player must press button when scale is close enough to match scale
    private void RingMatch(int difficultyScale)
    {
        bRingGameActive = true;
        ringMatchGame.SetActive(true);
        pActions.PlayerActions.Mouse1.started += MiniGameClick;
        StartCoroutine(ringMatchTiming(difficultyScale));
    }
    IEnumerator ringMatchTiming(int difficultyScale)
    {
        while (ringToScaleObject.localScale.x < ringToMatchObject.localScale.x + 0.4f && bRingGameActive)
        {
            if(ringToScaleObject.localScale.x < ringToMatchObject.localScale.x + 0.1f && ringToScaleObject.localScale.x > ringToMatchObject.localScale.x - 0.1f)
            {
                bInSpace = true;
            }
            else
            {
                bInSpace = false;
            }
            yield return new WaitForFixedUpdate();
            ringToScaleObject.localScale = new Vector2(ringToScaleObject.localScale.x + (0.01f * difficultyScale), ringToScaleObject.localScale.y + (0.01f) * difficultyScale);
        }

        if (bRingGameActive)
        {
            FailedClick();
        }
    }

    private void MiniGameClick(InputAction.CallbackContext c)
    {
        if (bInSpace)
        {
            SuccessClick();
        }
        else
        {
            FailedClick();
        }
    }
    private void SuccessClick()
    {
        Debug.Log("yes");
        totalWonGames++;
        CancelGame();
    }
    private void FailedClick()
    {
        Debug.Log("fail");
        CancelGame();
    }
    private void CancelGame()
    {
        pActions.PlayerActions.Mouse1.started -= MiniGameClick;
        bInSpace = false;
        //continue if max games not reached
        roundOfGames--;
        if(roundOfGames > 0)
        {
            StartMiniGame(difficultyScale);
        }
        else
        {
            EndGameRound(totalWonGames);
            //end of roun
            totalWonGames = 0;
            roundOfGames = 3;

        }

        //ring game cancel
        bRingGameActive = false;
        ringToScaleObject.localScale = new Vector3(0.1f, 0.1f);
        ringMatchGame.SetActive(false);
    }

    private void EndGameRound(int wonGames)
    {
        Debug.Log("Games Won: " + wonGames);
        //adjust sliders as needed here





    }
}
